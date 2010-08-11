using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Win.Core;
using DevExpress.ExpressApp.Win.Core.ModelEditor;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Model;
using eXpand.Utils.DependentAssembly;
using System.Linq;
using ResourcesModelStore = eXpand.ExpressApp.ModelDifference.Core.ResourcesModelStore;

namespace eXpand.ExpressApp.ModelEditor {
	public class MainClass {
		private static ModelEditorForm modelEditorForm;
		static private void HandleException(Exception e) {
			Tracing.Tracer.LogError(e);
			Messaging.GetMessaging(null).Show(ModelEditorForm.Title, e);
		}
		static private void OnException(object sender, ThreadExceptionEventArgs e) {
			HandleException(e.Exception);
		}
		[STAThread]
		static void Main(string[] args) {
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += OnException;

		    try {
		        var pathInfo = new PathInfo(args);
		        CheckAssemblyFile(pathInfo);
		        ApplicationModulesManager applicationModulesManager = GetApplicationModulesManager(pathInfo);
		        IModelApplication modelApplication = GetModelApplication(applicationModulesManager, pathInfo);
		        ModelEditorViewController controller = GetController(pathInfo, applicationModulesManager, modelApplication);
		        modelEditorForm = new ModelEditorForm(controller,new SettingsStorageOnRegistry(@"Software\Developer Express\eXpressApp Framework\Model Editor"));
		        modelEditorForm.Disposed +=(sender, eventArgs) => ((IModelEditorSettings) modelEditorForm).ModelEditorSaveSettings();
		        modelEditorForm.SetCaption(Path.GetFileName(pathInfo.LocalPath));

		        Application.Run(modelEditorForm);
		    }
		    catch (Exception exception) {
		        HandleException(exception);
		    }
			
		}

	    static ModelEditorViewController GetController(PathInfo pathInfo, ApplicationModulesManager applicationModulesManager, IModelApplication modelApplication) {
	        var fileModelStore = new FileModelStore(Path.GetDirectoryName(pathInfo.LocalPath), Path.GetFileNameWithoutExtension(pathInfo.LocalPath));
	        var unusableStore = new FileModelStore(Path.GetDirectoryName(pathInfo.LocalPath), ModelStoreBase.UnusableDiffDefaultName);
	        return new ModelEditorViewController(modelApplication, fileModelStore, unusableStore,applicationModulesManager.Modules);
	    }

	    static IModelApplication GetModelApplication(ApplicationModulesManager applicationModulesManager, PathInfo pathInfo) {
	        var modelManager = new ApplicationModelsManager(applicationModulesManager.Modules, applicationModulesManager.ControllersManager, applicationModulesManager.DomainComponents);
	        IModelApplication modelApplication = modelManager.CreateModelApplication();
            if (!(pathInfo.LocalPath.EndsWith(ModelStoreBase.ModelDiffDefaultName + ".xafml")))
                AddLayers((ModelApplicationBase)modelApplication, applicationModulesManager, pathInfo);
	        return modelApplication;
	    }

	    static ApplicationModulesManager GetApplicationModulesManager(PathInfo pathInfo) {
	        var designerModelFactory = new DesignerModelFactory();
	        ApplicationModulesManager applicationModulesManager = designerModelFactory.CreateModelManager(pathInfo.AssemblyPath, string.Empty);
	        applicationModulesManager.Load();
	        return applicationModulesManager;
	    }

	    static void CheckAssemblyFile(PathInfo pathInfo) {
	        if (!File.Exists(pathInfo.AssemblyPath)){
	            pathInfo.AssemblyPath = Path.Combine(Environment.CurrentDirectory, pathInfo.AssemblyPath);
	            if (!File.Exists(pathInfo.AssemblyPath)){
	                throw new Exception(String.Format("The file '{0}' couldn't be found.", pathInfo.AssemblyPath));
	            }
	        }
	    }

	    static void AddLayers(ModelApplicationBase modelApplication, ApplicationModulesManager applicationModulesManager, PathInfo pathInfo) {
            IEnumerable<string> assemblyPaths = DependentAssemblyPathResolver.GetAssemblyPaths(pathInfo.AssemblyPath).Reverse();
	        string resourceName = null;
	        foreach (var assemblyPath in assemblyPaths) {
	            var assembly = GetAssembly(applicationModulesManager, assemblyPath);
	            string addLayerCore = AddLayerCore(pathInfo, modelApplication, assembly);
                if (addLayerCore != null)
                    resourceName = addLayerCore;
	        }
	        var lastLayer = modelApplication.CreatorInstance.CreateModelApplication();
	        modelApplication.AddLayer(lastLayer);
            new ModelXmlReader().ReadFromResource(lastLayer,"",GetAssembly(applicationModulesManager, pathInfo.AssemblyPath), resourceName);
	    }

	    static string AddLayerCore(PathInfo pathInfo, ModelApplicationBase modelApplication, Assembly assembly) {
	        string resourceName = null;
	        var layer = modelApplication.CreatorInstance.CreateModelApplication();
	        modelApplication.AddLayer(layer);
	        var resourcesModelStore = new ResourcesModelStore(assembly, "");
	        resourcesModelStore.ResourceLoading += (sender, args) => {
	            if (args.ResourceName.EndsWith(Path.GetFileName(pathInfo.LocalPath))) {
	                args.Cancel = true;
	                resourceName = args.ResourceName;
	            }
	        };
	        resourcesModelStore.Load(layer);
            return resourceName;
	    }

	    static Assembly GetAssembly(ApplicationModulesManager applicationModulesManager, string path) {
	        return applicationModulesManager.Modules.Where(mbase => mbase.GetType().Assembly.Location==path).Select(mbase => mbase.GetType().Assembly).Single();
	    }
	}
}