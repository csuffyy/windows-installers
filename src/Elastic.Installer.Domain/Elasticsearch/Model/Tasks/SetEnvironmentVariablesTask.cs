﻿using Elastic.Installer.Domain.Session;
using System;
using System.IO.Abstractions;

namespace Elastic.Installer.Domain.Elasticsearch.Model.Tasks
{
	public class SetEnvironmentVariablesTask : InstallationTask
	{
		public SetEnvironmentVariablesTask(string[] args, ISession session) : base(args, session) { }
		public SetEnvironmentVariablesTask(InstallationModel model, ISession session, IFileSystem fileSystem)
			: base(model, session, fileSystem) { }

		protected override bool ExecuteTask()
		{
			this.Session.SendActionStart(1000, ActionName, "Setting environment variables", "[1]");
			string javaHome;
			if (!this.InstallationModel.JavaConfiguration.SetJavaHome(out javaHome))
			{
				throw new Exception($"A Java installation was detected, but unable to set the JAVA_HOME environment variable.  " +
									$"Attempted to set: '{javaHome}'");
			}

			var installDirectory = this.InstallationModel.LocationsModel.InstallDir;
			var configDirectory = this.InstallationModel.LocationsModel.ConfigDirectory;

			var esState = this.InstallationModel.ElasticsearchEnvironmentState;
			esState.SetEsHomeEnvironmentVariable(installDirectory);
			esState.SetEsConfigEnvironmentVariable(configDirectory);
			this.Session.SendProgress(1000, "Environment variables set");
			return true;
		}
	}
}