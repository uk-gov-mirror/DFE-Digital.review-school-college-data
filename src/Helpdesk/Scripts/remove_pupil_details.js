var Rscd = window.Rscd || {};
(function () {

  // Code to run in the form OnLoad event
  this.removePupilFormOnLoad = function (executionContext) {
    const formContext = executionContext.getFormContext();

    const removePupilForm = formContext.ui.quickForms.get('Remove pupil details');

    if (removePupilForm === undefined) {
      return;
    }

    this.initRemovePupilForm(formContext, removePupilForm);
  }

  this.initRemovePupilForm = function (formContext, removePupilForm) {
    if (!removePupilForm.isLoaded()) {
      setTimeout(this.initRemovePupilForm.bind(this), 10, formContext, removePupilForm);
    }
    else {
      const reasonCode = removePupilForm.getControl('rscd_reasoncode').getAttribute().getValue();

      switch (reasonCode) {
        // case 325: // Not at the end of 16 to 18 study
        //   break;

        // case 326: // International student
        //   break;

        // case 327: // Deceased
        //   break;

        case 328: // Not on roll
          this.showFields(removePupilForm, ['rscd_allocationyeardescription']);
          break;

        case 329: // Other with evidence
          this.showFields(removePupilForm, ['rscd_details']);
          break;

        // case 330: // Other without evidence
        //   break;

          default:
          // no additional fields to show
      }
      
    }
  }

  this.showFields = function (removePupilForm, fieldsToShow) {
    fieldsToShow.forEach(function(field){
      removePupilForm.getControl(field).setVisible(true);
  });
  }
}).call(Rscd);
