using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Configuration;
using Amazon.CloudWatch;

namespace CloudWatchMonitor
{
	public partial class MonitorService : ServiceBase
	{
		// Constants
		const string CloudWatchNamespace = "System/Windows";

		EventLog _eventLog = null;

		ManualResetEvent _evStop = new ManualResetEvent(false);

		string _instanceId;

		string _regionName;

		public MonitorService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			string eventLogSource = "AWS CloudWatch";
			if (!EventLog.SourceExists(eventLogSource))
			{
				EventLog.CreateEventSource(eventLogSource, "AWS CloudWatch");
			}
			_eventLog = new EventLog();
			_eventLog.Source = eventLogSource;

			new System.Threading.Thread(new ThreadStart(Run)).Start();

		}

		protected override void OnStop()
		{
			_evStop.Set();
		}

		private void Info(string message, params Object[] args)
		{
		
			if (_eventLog == null)
			{
				Console.WriteLine(message, args);
			}
			else
			{

            }
		}

		private void Error(string message, params Object[] args)
		{
			
			if (_eventLog == null)
			{
				Console.WriteLine("E:" + message, args);
			}
			else
			{
				string finalMessage = String.Format(message, args);
				_eventLog.WriteEntry(finalMessage, EventLogEntryType.Error);
			}
		}

		private bool ReadBoolean(string name, bool defaultValue)
		{
			string temp = ConfigurationManager.AppSettings[name];
			if (String.IsNullOrEmpty(temp))
				return defaultValue;
				

			bool result = false;
			if (!Boolean.TryParse(temp, out result))
				throw new Exception(String.Format("{0} must be True or False: {1}", name, temp));
			
			return result;
		}

		private int ReadInt(string name, int defaultValue)
		{
			string temp = ConfigurationManager.AppSettings[name];
			if (String.IsNullOrEmpty(temp))
				return defaultValue;

			int result = 0;
			if (!Int32.TryParse(temp, out result))
				throw new Exception(String.Format("{0} must be a number: {1}", name, temp));

			return result;
		}

		private string ReadString(string name, string defaultValue)
		{
			string temp = ConfigurationManager.AppSettings[name];
			if (String.IsNullOrEmpty(temp))
				return defaultValue;
			return temp;
		}

		private List<string> ReadStringList(string name, List<string> defaultValue)
		{
			string temp = ConfigurationManager.AppSettings[name];
			if (String.IsNullOrEmpty(temp))
				return defaultValue;

			string[] values = temp.Split(',');
			return values.Select(s => s.Trim()).Where(s => !String.IsNullOrEmpty(s)).ToList();
		}


		bool _isSubmitMemoryAvailable;
		bool _isSubmitMemoryUsed;
		bool _isSubmitMemoryUtilization;
		bool _isSubmitPhysicalMemoryAvailable;
		bool _isSubmitPhysicalMemoryUsed;
		bool _isSubmitPhysicalMemoryUtilization;
		bool _isSubmitVirtualMemoryAvailable;
		bool _isSubmitVirtualMemoryUsed;
		bool _isSubmitVirtualMemoryUtilization;
		
		public void Run()
		{
			Info("AWS CloudWatch starting");
			
			int monitorPeriodInMinutes = 1;

			try
			{
				Info("Loading Configuration");

				monitorPeriodInMinutes = ReadInt("MonitorPeriodInMinutes", 1);

			
				if (monitorPeriodInMinutes < 1)
					throw new Exception("MonitorPeriodInMinutes must be greater than or equal to 1");
				Info("MonitorPeriodInMinutes: {0}", monitorPeriodInMinutes);


				_isSubmitMemoryAvailable = ReadBoolean("SubmitMemoryAvailable", true);
				Info("SubmitMemoryAvailable: {0}", _isSubmitMemoryAvailable);

				_isSubmitMemoryUsed = ReadBoolean("SubmitMemoryUsed", true);
				Info("SubmitMemoryUsed: {0}", _isSubmitMemoryUsed);

				_isSubmitMemoryUtilization = ReadBoolean("SubmitMemoryUtilization", true);
				Info("SubmitMemoryUtilization: {0}", _isSubmitMemoryUtilization);

				_isSubmitPhysicalMemoryAvailable = ReadBoolean("SubmitPhysicalMemoryAvailable", true);
				Info("SubmitPhysicalMemoryAvailable: {0}", _isSubmitPhysicalMemoryAvailable);

				_isSubmitPhysicalMemoryUsed = ReadBoolean("SubmitPhysicalMemoryUsed", true);
				Info("SubmitPhysicalMemoryUsed: {0}", _isSubmitPhysicalMemoryUsed);

				_isSubmitPhysicalMemoryUtilization = ReadBoolean("SubmitPhysicalMemoryUtilization", true);
				Info("SubmitPhysicalMemoryUtilization: {0}", _isSubmitPhysicalMemoryUtilization);

				_isSubmitVirtualMemoryAvailable = ReadBoolean("SubmitVirtualMemoryAvailable", true);
				Info("SubmitVirtualMemoryAvailable: {0}", _isSubmitVirtualMemoryAvailable);

				_isSubmitVirtualMemoryUsed = ReadBoolean("SubmitVirtualMemoryUsed", true);
				Info("SubmitVirtualMemoryUsed: {0}", _isSubmitVirtualMemoryUsed);

				_isSubmitVirtualMemoryUtilization = ReadBoolean("SubmitVirtualMemoryUtilization", true);
				Info("SubmitVirtualMemoryUtilization: {0}", _isSubmitVirtualMemoryUtilization);


				_instanceId = ReadString("InstanceId", null);
				if (!String.IsNullOrEmpty(_instanceId))
					Info("Instance ID: {0}", _instanceId);

				_regionName = ReadString("AWSRegion", null);
				if (!String.IsNullOrEmpty(_regionName))
					Info("Region: {0}", _regionName);
			}
			catch (Exception e)
			{
				Error(e.Message);
				if (!Environment.UserInteractive)
					this.Stop(); // Tell the service to stop
				return;
			}

			if (!_isSubmitMemoryAvailable &&
				!_isSubmitMemoryUsed &&
				!_isSubmitMemoryUtilization &&
				!_isSubmitPhysicalMemoryAvailable &&
				!_isSubmitPhysicalMemoryUsed &&
				!_isSubmitPhysicalMemoryUtilization &&
				!_isSubmitVirtualMemoryAvailable &&
				!_isSubmitVirtualMemoryUsed &&
				!_isSubmitVirtualMemoryUtilization)
			{
				Error("No data is selected to submit.");
				if (!Environment.UserInteractive)
					this.Stop(); 
				return;
			}

			while (true)
			{
				DateTime updateBegin = DateTime.Now;

				try 
				{	        
					UpdateMetrics();
				}
				catch (Exception e)
				{
					Error("Error submitting metrics: {0}", e.Message);

				}

				DateTime updateEnd = DateTime.Now;
				TimeSpan updateDiff = (updateEnd - updateBegin);

				TimeSpan baseTimeSpan = TimeSpan.FromMinutes(monitorPeriodInMinutes);

				TimeSpan timeToWait = (baseTimeSpan - updateDiff);
				if (timeToWait.TotalMilliseconds < 50)
					timeToWait = TimeSpan.FromMilliseconds(50);

				if (_evStop.WaitOne(timeToWait, false))
					break;
			}

			Info("AWS CloudWatch shutting down");
		}

		private void UpdateMetrics()
		{
			if (!PopulateInstanceId())
				return;

			if (!PopulateRegion())
				return;

			List<Amazon.CloudWatch.Model.MetricDatum> metrics = new List<Amazon.CloudWatch.Model.MetricDatum>();
		
			

			if (_isSubmitMemoryAvailable ||
				_isSubmitMemoryUsed ||
				_isSubmitMemoryUtilization ||
				_isSubmitPhysicalMemoryAvailable ||
				_isSubmitPhysicalMemoryUsed ||
				_isSubmitPhysicalMemoryUtilization ||
				_isSubmitVirtualMemoryAvailable ||
				_isSubmitVirtualMemoryUsed ||
				_isSubmitVirtualMemoryUtilization)
			{
				SubmitMemoryMetrics(metrics);
			}

			Info("\tSubmitting Memory Stats");

			var client = CreateClient();

			for (int skip = 0; ; skip += 20)
			{
				var metricsThisRound = metrics.Skip(skip).Take(20);
				if (!metricsThisRound.Any())
					break;

				var request = new Amazon.CloudWatch.Model.PutMetricDataRequest()
				{
					Namespace = CloudWatchNamespace,
					MetricData = metricsThisRound.ToList()
				};
				var response = client.PutMetricData(request);
			}

			Info("Successful");
		}

		private IAmazonCloudWatch CreateClient()
		{
			AmazonCloudWatchConfig config = new AmazonCloudWatchConfig()
			{
				RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_regionName)
			};

			return new AmazonCloudWatchClient(config);
		}

		private void SubmitMemoryMetrics(List<Amazon.CloudWatch.Model.MetricDatum> metrics)
		{
			Info("Adding Memory Stats");

			var dimensions = new List<Amazon.CloudWatch.Model.Dimension>();
			dimensions.Add(new Amazon.CloudWatch.Model.Dimension()
			{
				Name = "InstanceId", 
				Value = _instanceId
			});

		
			var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();

			double availablePhysicalMemory = computerInfo.AvailablePhysicalMemory;
			double totalPhysicalMemory = computerInfo.TotalPhysicalMemory;
			double physicalMemoryUsed = (totalPhysicalMemory - availablePhysicalMemory);
			double physicalMemoryUtilized = (physicalMemoryUsed / totalPhysicalMemory) * 100;

			Info("\tTotal Physical Memory: {0:N0} bytes", totalPhysicalMemory);

			if (_isSubmitPhysicalMemoryUsed)
			{
				Info("\tPhysical Memory Used: {0:N0} bytes", physicalMemoryUsed);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "PhysicalMemoryUsed",
					Unit = "Bytes",
					Value = physicalMemoryUsed,
					Dimensions = dimensions
				});
			}

			if (_isSubmitPhysicalMemoryAvailable)
			{
				Info("\tAvailable Physical Memory: {0:N0} bytes", availablePhysicalMemory);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "PhysicalMemoryAvailable",
					Unit = "Bytes",
					Value = availablePhysicalMemory,
					Dimensions = dimensions
				});
			}

			if (_isSubmitPhysicalMemoryUtilization)
			{
				Info("\tPhysical Memory Utilization: {0:F1}%", physicalMemoryUtilized);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "PhysicalMemoryUtilization",
					Unit = "Percent",
					Value = physicalMemoryUtilized,
					Dimensions = dimensions
				});
			}

			double availableVirtualMemory = computerInfo.AvailableVirtualMemory;
			double totalVirtualMemory = computerInfo.TotalVirtualMemory;
			double virtualMemoryUsed = (totalVirtualMemory - availableVirtualMemory);
			double virtualMemoryUtilized = (virtualMemoryUsed / totalVirtualMemory) * 100;

			Info("\tTotal Virtual Memory: {0:N0} bytes", totalVirtualMemory);

			if (_isSubmitVirtualMemoryUsed)
			{
				Info("\tVirtual Memory Used: {0:N0} bytes", physicalMemoryUsed);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "VirtualMemoryUsed",
					Unit = "Bytes",
					Value = virtualMemoryUsed,
					Dimensions = dimensions
				});
			}

			if (_isSubmitVirtualMemoryAvailable)
			{
				Info("\tAvailable Virtual Memory: {0:N0} bytes", availableVirtualMemory);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "VirtualMemoryAvailable",
					Unit = "Bytes",
					Value = availableVirtualMemory,
					Dimensions = dimensions
				});
			}

			if (_isSubmitVirtualMemoryUtilization)
			{
				Info("\tVirtual Memory Utilization: {0:F1}%", virtualMemoryUtilized);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "VirtualMemoryUtilization",
					Unit = "Percent",
					Value = virtualMemoryUtilized,
					Dimensions = dimensions
				});
			}

			double availableMemory = availablePhysicalMemory + availableVirtualMemory;
			double totalMemory = totalPhysicalMemory + totalVirtualMemory;
			double memoryUsed = (totalMemory - availableMemory);
			double memoryUtilized = (memoryUsed / totalMemory) * 100;

			Info("\tTotal Memory: {0:N0} bytes", totalMemory);

			if (_isSubmitMemoryUsed)
			{
				Info("\tMemory Used: {0:N0} bytes", physicalMemoryUsed);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "MemoryUsed",
					Unit = "Bytes",
					Value = memoryUsed,
					Dimensions = dimensions
				});
			}

			if (_isSubmitMemoryAvailable)
			{
				Info("\tAvailable Memory: {0:N0} bytes", availableMemory);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "MemoryAvailable",
					Unit = "Bytes",
					Value = availableMemory,
					Dimensions = dimensions
				});
			}

			if (_isSubmitMemoryUtilization)
			{
				Info("\tMemory Utilization: {0:F1}%", memoryUtilized);
				metrics.Add(new Amazon.CloudWatch.Model.MetricDatum()
				{
					MetricName = "MemoryUtilization",
					Unit = "Percent",
					Value = memoryUtilized,
					Dimensions = dimensions
				});
			}
		}

		private bool PopulateInstanceId()
		{
			
			if (!String.IsNullOrEmpty(_instanceId))
				return true;
			
			try
			{
				// Get the instance id
				Uri uri = new Uri("http://169.254.169.254/latest/meta-data/instance-id");

				var client = new System.Net.WebClient();
				_instanceId = client.DownloadString(uri);

				Info("Instance ID: {0}", _instanceId);
				return true;
			}
			catch (Exception e)
			{
				Error("Error getting instance id: {0}", e.Message);
				return false;
			}
		}

		private bool PopulateRegion()
		{
			if (!String.IsNullOrEmpty(_regionName))
				return true;

			string availabilityZone;
			try
			{
				Uri uri = new Uri("http://169.254.169.254/latest/meta-data/placement/availability-zone");

				var client = new System.Net.WebClient();
				availabilityZone = client.DownloadString(uri);

				Info("Availability Zone: {0}", availabilityZone);
			}
			catch (Exception e)
			{
				Error("Error getting availability zone: {0}", e.Message);
				return false;
			}

			
			_regionName = availabilityZone.Substring(0, availabilityZone.Length - 1);
			Info("Region: {0}", _regionName);

			return true;
		}
	}
}
