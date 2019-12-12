using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CSharpBNP.Business;
using CSharpBNP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Configuration;

namespace CSharpBNP.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDialog _dialog;
        private readonly ITransaction _transaction;
        private string _xmlFile;
        private string _logFile;
        private string _csvPath;

        #endregion

        #region Properties

        public string XmlFile
        {
            get { return _xmlFile; }
            set
            {
                _xmlFile = value;
                RaisePropertyChanged("XmlFile");
            }
        }

        public string LogFile
        {
            get { return _logFile; }
            set
            {
                _logFile = value;
                RaisePropertyChanged("LogFile");
            }
        }

        public string CsvPath
        {
            get { return _csvPath; }
            set
            {
                _csvPath = value;
                RaisePropertyChanged("CsvPath");
            }
        }

        #endregion

        #region Command

        public RelayCommand OpenXmlFileDialogCommand { get; private set; }
        public RelayCommand OpenCsvFolderBrowserDialogCommand { get; private set; }
        public RelayCommand ExecuteCommand { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDialog dialog, ITransaction transaction)
        {
            _dialog = dialog;
            _transaction = transaction;
            InitializeValue();
            InitialiazeCommand();
        }

        #endregion

        #region Methods

        private void InitializeValue()
        {
            XmlFile = ConfigurationManager.AppSettings["XmlFile"];
            CsvPath = ConfigurationManager.AppSettings["CsvPath"];
        }

        private void InitialiazeCommand()
        {
            OpenXmlFileDialogCommand = new RelayCommand(OpenXmlFileDialog);
            OpenCsvFolderBrowserDialogCommand = new RelayCommand(OpenCsvFolderBrowserDialog);
            ExecuteCommand = new RelayCommand(Execute);
        }

        private void OpenXmlFileDialog()
        {
            string openFileDialog = _dialog.OpenFileDialogShow("XML Files (*.xml)|*.xml");
            if (!string.IsNullOrWhiteSpace(openFileDialog))
            {
                XmlFile = openFileDialog;
            }

        }

        private void OpenCsvFolderBrowserDialog()
        {
            string folderBrowserDialog = _dialog.FolderBrowserDialogShow(CsvPath);
            if (!string.IsNullOrWhiteSpace(folderBrowserDialog))
            {
                CsvPath = folderBrowserDialog;
            }
        }

        private void Execute()
        {
            LogUtils.LogInformation("CSharpBNP Start");
            DateTime dateTime = DateTime.Now;

            List<Trade> trades = DeserializeXmlFile();
            List<Output> outputs = GetOutputs(trades);
            CreateCSV(outputs);

            LogUtils.LogInformation($"CSharpBNP End => {(DateTime.Now- dateTime).Milliseconds} Milliseconds");
        }

        private List<Trade> DeserializeXmlFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Trade>), new XmlRootAttribute("Trades"));
                using (StreamReader sr = new StreamReader(XmlFile))
                {
                    return (List<Trade>)serializer.Deserialize(sr);
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                return new List<Trade>();
            }
        }

        private List<Output> GetOutputs(List<Trade> trades)
        {
            List<Output> outputs = new List<Output>();
            var groups = trades.GroupBy(x => x.CorrelationID);
            foreach (var group in groups)
            {
                outputs.Add(CreateOutput(group, group.Key));
            }
            return outputs;
        }

        private Output CreateOutput(IEnumerable<Trade> group, string key)
        {
            var numberOfTrades = group.First().NumberOfTrades;
            Output output = new Output { CorrelationID = key, NumberOfTrades = numberOfTrades };
            StateEnum state = _transaction.CheckTransactionCount(numberOfTrades, group.Count());
            //Si le state n'est pas accepter j'ai decidé de ne pas appeler le test de la limite
            if (state == StateEnum.Accepted)
            {
                var sum = group.Sum(x => x.Value);
                var limit = group.First().Limit;
                state = _transaction.CheckTransactionLimit(sum, limit);
            }
            else if (state == StateEnum.None)
            {
                //Le StateEnum.None indique une valeur non traiter par la fonction CheckTransactionCount
                LogUtils.LogInformation($"CorrelationID = {key} | state = {state}");

            }
            output.State = state;
            return output;
        }

        private void CreateCSV(List<Output> outputs)
        {
            try
            {
                string separator = ";";
                using (StreamWriter streamWriter = new StreamWriter($@"{CsvPath}\resultExample.csv"))
                {
                    streamWriter.WriteLine($"CorrelationID{separator}NumberOfTrades{separator}State");
                    foreach (var output in outputs.OrderBy(x => x.CorrelationID))
                    {
                        string line = $"{output.CorrelationID}{separator}{output.NumberOfTrades}{separator}{output.State}";
                        streamWriter.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
            }
        }

        #endregion
    }
}