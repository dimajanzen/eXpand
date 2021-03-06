using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using eXpand.ExpressApp.ModelDifference.DataStore.BaseObjects;

namespace eXpand.ExpressApp.ModelDifference.Controllers{
    public partial class _CreateNewDifferenceObjectViewController : ViewController
    {
        public _CreateNewDifferenceObjectViewController()
        {
            InitializeComponent();
            RegisterActions(components);
            TargetObjectType = typeof (ModelDifferenceObject);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            var controller = Frame.GetController<NewObjectViewController>();
            controller.ObjectCreating+=ControllerOnObjectCreating;
            controller.ObjectCreated+=OnObjectCreated;
        }

        [CoverageExclude]
        protected override void OnDeactivating()
        {
            base.OnDeactivating();
            Frame.GetController<NewObjectViewController>().ObjectCreated -= OnObjectCreated;
            Frame.GetController<NewObjectViewController>().ObjectCreating -= ControllerOnObjectCreating;
        }

        private void ControllerOnObjectCreating(object sender, ObjectCreatingEventArgs args){
            if (typeof(ModelDifferenceObject).IsAssignableFrom(args.ObjectType))
                throw new UserFriendlyException(new Exception("Only cloned is allowed"));
        }

        protected virtual void OnObjectCreated(object sender, ObjectCreatedEventArgs args){
            ((ModelDifferenceObject) args.CreatedObject).InitializeMembers(Guid.NewGuid().ToString());
        }
    }
}