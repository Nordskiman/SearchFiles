using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SearchFiles
{
    public delegate void TreeUpdate(string path);
    public delegate void StatusUpdate(string currentFile, bool isSearchEnded);

    public interface ISearchService
    {
        event TreeUpdate OnTreeUpdate;
        event StatusUpdate OnStatusUpdate;
        Task<int> StartSearch(string folder, string pattern, string searchText, CancellationTokenSource cancel);
        void CancelSearch();
    }

    public class SearchService : ISearchService
    {
        private CancellationTokenSource _cancelSource;

        public event TreeUpdate OnTreeUpdate;
        public event StatusUpdate OnStatusUpdate;

        private int _proceededFiles;

        public async Task<int> StartSearch(string path, string pattern, string searchText, CancellationTokenSource cancelSource)
        {
            _cancelSource = cancelSource;
            _proceededFiles = 0;

            await GetDirectoryAsync(new DirectoryInfo(path), pattern, searchText, _cancelSource.Token);

            OnStatusUpdate?.Invoke("Поиск окончен", true);

            return _proceededFiles;
        }

        private async Task GetDirectoryAsync(DirectoryInfo rootFolder, string pattern, string searchText, CancellationToken cancellation)
        {
            FileInfo[] files = null;
            try
            {
                files = rootFolder.GetFiles(pattern);
            }
            catch (Exception)
            {

            }

            if (files != null)
            {
                foreach (var fi in files)
                    if (fi.Directory != null) await ProcessFileAsync(fi.FullName, searchText, cancellation);

                var subDirs = rootFolder.GetDirectories();

                foreach (var dirInfo in subDirs)
                    await GetDirectoryAsync(dirInfo, pattern, searchText, cancellation);
            }
        }

        private async Task ProcessFileAsync(string path, string searchText, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
                await Task.FromResult(false);

            var fileName = Path.GetFileName(path);
            var searchString = searchText.Trim();
            if (File.ReadLines(path).Any(line => line.Contains(searchString)) && !cancelToken.IsCancellationRequested)
                ResultFound(path);

            if (OnStatusUpdate != null && !cancelToken.IsCancellationRequested)
                OnStatusUpdate(fileName, false);

            _proceededFiles++;
            await Task.FromResult(true);
        }

        private void ResultFound(string path)
        {
            OnTreeUpdate?.Invoke(path);
        }

        public void CancelSearch()
        {
            _cancelSource?.Cancel();
        }
    }
}
