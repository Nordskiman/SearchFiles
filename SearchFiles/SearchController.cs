using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace SearchFiles
{
    public interface IPresenter
    {
        void Run();
    }

    public class SearchController : IPresenter
    {
        private const string StartBtnText = "Начать поиск";
        private const string CancelBtnText = "Отменить поиск";

        private const string TotalFilesText = "Обработано файлов: ";
        private const string CurrentFileText = " Текущий файл: ";

        private const int TreeDelayUpdateTime = 30;

        private int _totalFiles;

        private readonly ISearchView _view;
        private readonly ISearchService _service;

        private ConcurrentQueue<string> _searchResultsPaths;
        private CancellationTokenSource _cancelSource;

        private Timer _timer;
        private Timer _resultsTimer;

        private DateTime _startTime;
        private DateTime _stopTime;

        public SearchController(ISearchView view, ISearchService service)
        {
            _view = view;
            _service = service;
            _view.StartSearch += ViewOnStartSearch;
            _service.OnTreeUpdate += path =>
            {
                _searchResultsPaths.Enqueue(path);
            };
            _view.Closing += () =>
            {
                Settings.Save(new SavedData()
                {
                    StartDirectory = _view.Directory.Text,
                    FilePattern = _view.Pattern.Text,
                    TextToSearch = _view.SearchText.Text
                });
            };
        }

        private void ViewOnStartSearch()
        {
            if (_view.Start.Text.Equals(CancelBtnText))
            {
                ResetSearch();
                return;
            }

            StartSearch(_view.Directory.Text, _view.Pattern.Text, _view.SearchText.Text);
        }

        private void OnStatusUpdate(string currentFile, bool isSearchEnded)
        {
            if (isSearchEnded)
                ResetSearch();

            if (isSearchEnded && _totalFiles == 0)
                MessageBox.Show(@"Файлы по шаблону не найдены", @"Проверьте шаблон файла. Можно использовать звёздочки (*), знаки вопроса (?) и квадратные скобки ([]).");

            _totalFiles++;

            if (!_cancelSource.IsCancellationRequested)
                UpdateLabels(_totalFiles, currentFile);
        }

        private void StartSearch(string folder, string pattern, string searchText)
        {
            _service.OnStatusUpdate += OnStatusUpdate;

            if (string.IsNullOrWhiteSpace(folder))
            {
                MessageBox.Show(@"Выберите папку", @"Пожалуйста, выберите папку для поиска файлов");
                return;
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                MessageBox.Show(@"Заполните шаблон", @"Пожалуйста, заполните шаблон для поиска файлов");
                return;
            }

            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show(@"Заполните текст для поиска", @"Пожалуйста, заполните текст для поиска");
                return;
            }

            Settings.Save(new SavedData { StartDirectory = folder, FilePattern = pattern, TextToSearch = searchText });

            UpdateStartBtn(CancelBtnText);
            NewSearch();
            _searchResultsPaths = new ConcurrentQueue<string>();
            _timer = new Timer(100);
            _timer.Elapsed += OnTimerStatusUpdate;
            _timer.Start();
            _startTime = DateTime.Now;
            _resultsTimer = new Timer(TreeDelayUpdateTime);
            _resultsTimer.Elapsed += OnResultsTimerUpdate;
            _resultsTimer.Start();
            _cancelSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                try
                {
                    _service.StartSearch(folder, pattern, searchText, _cancelSource);
                }
                catch (Exception)
                {
                    MessageBox.Show(@"Ошибка", @"Неизвестная ошибка при поиске");
                }
            });
        }

        private void NewSearch()
        {
            _view.Tree.Nodes.Clear();

            UpdateLabels(0, "-");

            _totalFiles = 0;

            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }

            if (_resultsTimer != null)
            {
                _resultsTimer.Stop();
                _resultsTimer = null;
            }
        }

        private void OnResultsTimerUpdate(object sender, ElapsedEventArgs e)
        {
            if (_searchResultsPaths.Count <= 0) return;

            string path;
            _searchResultsPaths.TryDequeue(out path);

            if (!string.IsNullOrWhiteSpace(path))
                _view.Context.Post(delegate { PopulateTree(_view.Tree, path, '\\'); }, null);
        }

        private void OnTimerStatusUpdate(object sender, ElapsedEventArgs e)
        {
            _stopTime = DateTime.Now;
            var elapsed = _stopTime - _startTime;

            _view.Context.Post(delegate
            {
                _view.TimeLabel.Text = elapsed.ToString(@"hh\:mm\:ss");
            }, null);
        }

        private void UpdateStartBtn(string text)
        {
            _view.Context.Post(delegate
            {
                _view.Start.Text = text;
                _view.Start.Refresh();
            }, null);
        }

        private void UpdateLabels(int totalFiles, string currentFile)
        {
            _view.Context.Post(delegate
            {
                _view.FileLabel.Text = TotalFilesText + totalFiles + CurrentFileText + currentFile;
            }, null);
        }

        private void ResetSearch()
        {
            if (_cancelSource != null)
                _service.CancelSearch();

            _service.OnStatusUpdate -= OnStatusUpdate;

            UpdateStartBtn(StartBtnText);

            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }
        }

        private static void PopulateTree(TreeView treeView, string path, char pathSeparator)
        {
            TreeNode lastNode = null;

            var subPathAgg = string.Empty;
            foreach (var subPath in path.Split(pathSeparator))
            {
                subPathAgg += subPath + pathSeparator;
                var nodes = treeView.Nodes.Find(subPathAgg, true);

                if (nodes.Length == 0)
                    lastNode = lastNode?.Nodes.Add(subPathAgg, subPath, 0) ?? treeView.Nodes.Add(subPathAgg, subPath, 0);
                else
                    lastNode = nodes[0];
            }
        }

        public void Run()
        {
            var settings = Settings.Load();

            _view.Context.Post(delegate
            {
                _view.Directory.Text = settings.StartDirectory;
                _view.Directory.Refresh();

                _view.Pattern.Text = settings.FilePattern;
                _view.Pattern.Refresh();

                _view.SearchText.Text = settings.TextToSearch;
                _view.SearchText.Refresh();

            }, null);

            _view.Show();
        }
    }

    public class SavedData
    {
        public string StartDirectory { get; set; }
        public string FilePattern { get; set; }
        public string TextToSearch { get; set; }
    }

    public static class Settings
    {
        private const string PathFile = "../../settings.ini";

        public static void Save(SavedData saveData)
        {
            var text = saveData.StartDirectory + "," + saveData.FilePattern + "," + saveData.TextToSearch;
            File.WriteAllText(PathFile, text);
        }

        public static SavedData Load()
        {
            try
            {
                var readText = File.ReadAllLines(PathFile);
                var list = readText.Select(s => s.Split(',')).ToList();
                SavedData savedData = null;
                foreach (var t in list)
                    savedData = new SavedData
                    {
                        StartDirectory = t[0].Trim(),
                        FilePattern = t[1].Trim(),
                        TextToSearch = t[2].Trim()
                    };

                return savedData;
            }
            catch (FileNotFoundException)
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                return new SavedData { StartDirectory = folderPath, FilePattern = "*.txt", TextToSearch = "abc" };
            }
        }
    }
}
