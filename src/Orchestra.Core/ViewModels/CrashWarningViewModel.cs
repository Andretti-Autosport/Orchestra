// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrashWarningViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Services;

    public class CrashWarningViewModel : ViewModelBase
    {
        #region Fields

        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        private readonly IManageAppDataService _manageAppDataService;
        private readonly Assembly _assembly;
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly ILanguageService _languageService;
        private readonly IOpenFileService _openFileService;
        #endregion

        #region Constructors
        public CrashWarningViewModel(IManageAppDataService manageAppDataService, IMessageService messageService, INavigationService navigationService,
            ILanguageService languageService, IOpenFileService openFileService)
        {
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => manageAppDataService);
            Argument.IsNotNull(() => languageService);
            Argument.IsNotNull(() => openFileService);

            _manageAppDataService = manageAppDataService;
            _messageService = messageService;
            _navigationService = navigationService;
            _languageService = languageService;
            _openFileService = openFileService;

            _assembly = Catel.Reflection.AssemblyHelper.GetEntryAssembly();
            var directory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Andretti Autosport\\SimUI\\log"));
            FilePath = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().ToString();

            Continue = new TaskCommand(OnContinueExecuteAsync);
            Open = new TaskCommand(OnOpenExecuteAsync);
            Copy = new TaskCommand(OnCopyExecuteAsync);
        }
        #endregion

        #region Properties
        public override string Title
        {
            get { return _assembly.Title(); }
        }

        public string LogFile { get; set; }

        private string _filePath { get; set; }

        public string FilePath
        {
            get 
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                using (var streamReader = new StreamReader(_filePath))
                {
                    LogFile = streamReader.ReadToEnd();
                }
            }
        }
        #endregion

        #region Commands
        public TaskCommand Copy { get; set; }

        private async Task OnCopyExecuteAsync()
        {
            StringCollection files = new StringCollection();
            files.Add(FilePath);
            Clipboard.SetFileDropList(files);
        }

        public TaskCommand Open { get; set; }

        private async Task OnOpenExecuteAsync()
        {
            DetermineOpenFileContext context = new DetermineOpenFileContext
            {
                Filter = "Log|*.log",
                CheckFileExists = true,
                IsMultiSelect = false,
                Title = "Select Log File",
                InitialDirectory  = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Andretti Autosport\\SimUI\\log")
            };

            DetermineOpenFileResult result = await _openFileService.DetermineFileAsync(context);

            if (result.Result)
            {
                FilePath = result.FileName;
            }
        }

        public TaskCommand Continue { get; set; }

        private async Task OnContinueExecuteAsync()
        {
            Log.Info("User choose NOT to delete any data and continue (living on the edge)");

            await CloseViewModelAsync(false);
        }
        #endregion

    }
}
