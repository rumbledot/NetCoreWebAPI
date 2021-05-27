using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Services
{
    public class LogWatcherConfig
    {
        private string api_folder_location = Directory.GetCurrentDirectory();

        private const string base_log_filename = "logs";

        public string log_filename_warning
        {
            get 
            { 
                return $"{api_folder_location}\\{base_log_filename}_warning.txt";
            }
        }

        public string log_filename_exception
        {
            get
            {
                return $"{api_folder_location}\\{base_log_filename}_exception.txt";
            }
        }

        private bool _is_ready = false;
        public bool is_ready
        {
            get
            {
                return _is_ready;
            } 
        }

        public void Initialise()
        {
            _is_ready = CheckWarningLog() && CheckErrorLog();
        }

        private bool CheckWarningLog()
        {
            try
            {
                if (File.Exists(log_filename_warning))
                {
                    //using (StreamWriter fs = new StreamWriter(log_filename_warning))
                    //{
                    //    FileInfo log_file = new FileInfo(log_filename_warning);
                    //    if (log_file.Length > 50000000) //5 megs then reset the log file
                    //    {
                    //        fs.WriteLine($"[{DateTime.Now}]>[WARNING LOG]>INITIALISED");
                    //    }
                    //}

                    return true;
                }
                else
                {
                    using (StreamWriter fs = new StreamWriter(log_filename_warning))
                    {
                        fs.WriteLine($"[{DateTime.Now}]>[WARNING LOG]>INITIALISED");
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool CheckErrorLog()
        {
            try
            {
                if (File.Exists(log_filename_exception))
                {
                    //using (StreamWriter fs = new StreamWriter(log_filename_exception))
                    //{
                    //    FileInfo log_file = new FileInfo(log_filename_exception);
                    //    if (log_file.Length > 50000000) //5 megs then reset the log file
                    //    {
                    //        fs.WriteLine($"[{DateTime.Now}]>[EXCEPTION LOG]>INITIALISED");
                    //    }
                    //}

                    return true;
                }
                else
                {
                    using (StreamWriter fs = new StreamWriter(log_filename_exception))
                    {
                        fs.WriteLine($"[{DateTime.Now}]>[EXCEPTION LOG]>INITIALISED");
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async void LogError(string message)
        {
            if (_is_ready)
            {
                using (StreamWriter fs = new StreamWriter(log_filename_exception, append:true))
                {
                    await fs.WriteLineAsync(message);
                }
            }
        }

        public async void LogWarning(string message)
        {
            if (_is_ready)
            {
                using (StreamWriter fs = new StreamWriter(log_filename_warning, append: true))
                {
                    await fs.WriteLineAsync(message);
                }
            }
        }
    }
}
