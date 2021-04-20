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
console.log(reasonCode);
      switch (reasonCode) {
        // case 325: // Not at the end of 16 to 18 study
        //   break;
        // case 326: // International student
        //   break;
        // case 327: // Deceased
        //   break;
        case 328: // Not on roll
         this.showFields(removePupilForm, ['rscd_allocationyeardescription', 'rscd_allocationyear_1description', 'rscd_allocationyear_2description']);
          break;
        case 330: // Other without evidence
          this.showFields(removePupilForm, ['rscd_allocationyeardescription']);
          break;
        case 329: // Other with evidence
          this.showFields(removePupilForm, ['rscd_details']);
          break;
     case 8: // admitted from abroad English not 1st language
        this.showFields(removePupilForm, ['rscd_language', 'rscd_countryoforigin' ,'rscd_dateofarrival']);
        if (removePupilForm.getControl('rscd_dateonroll').getAttribute().getValue() !== null) {            
            removePupilForm.getControl('rscd_dateonroll').setVisible(true);
         }
        break;
    case 10: // Admitted following permanent exclusion from maintained school
         this.showFields(removePupilForm, ['rscd_laestabofexcludedschool', 'rscd_pupilexclusiondate', 'rscd_details']);
         break;
     case 11: // Perm left England
           var fieldsArray =  ['rscd_countrypupilleftenglandfor', 'rscd_dateoffroll'];
           var detailsValue = removePupilForm.getControl('rscd_details').getAttribute().getValue();
          if (detailsValue !== ''){
              fieldsArray.push('rscd_details');
          }
           this.showFields(removePupilForm, fieldsArray);
           break;
    case 12: // Deceased 
          this.showFields(removePupilForm, ['rscd_dateoffroll']);
          break;
    case 19: // Other  > sub reasons
            var fieldsArray = ['rscd_subreason', 'rscd_reasondescription'];
            var subReasonValue = removePupilForm.getControl('rscd_subreason').getAttribute().getValue();
console.log(subReasonValue);
            if (subReasonValue.toLowerCase().indexOf('elective home education') > -1){
                 removePupilForm.getControl('rscd_details').setVisible(true);
                if (removePupilForm.getControl('rscd_dateoffroll').getAttribute().getValue() !== '') {
                    removePupilForm.getControl('rscd_dateoffroll').setVisible(true);
                }
           }
           if (subReasonValue.toLowerCase().indexOf('eal exceptional circumstances') > -1){
                fieldsArray.push('rscd_language', 'rscd_countryoforigin', 'rscd_dateofarrival');
                if (removePupilForm.getControl('rscd_dateonroll').getAttribute().getValue() !== null) {  
                  removePupilForm.getControl('rscd_dateonroll').setVisible(true);
                 }
           }
           
          if (subReasonValue.toLowerCase().indexOf('permanently excluded from this school') > -1 || subReasonValue.toLowerCase().indexOf('missing in education') > -1 ){
               removePupilForm.getControl('rscd_dateoffroll').setVisible(true);
               removePupilForm.getControl('rscd_details').setVisible(true);
          }
            this.showFields(removePupilForm, fieldsArray);
            break;
          case 1902: // 
              var detailsValue = removePupilForm.getControl('rscd_details').getAttribute().getValue();
              if (detailsValue !== ''){
                 this.showFields(removePupilForm,['rscd_details']);
              }
              break;
           case 1903: //
               this.showFields(removePupilForm,['rscd_reasondescription']);
               break;
           case 13: //  dual registration
               removePupilForm.getControl('rscd_pupilsmainschoollaestab').setVisible(true);
               
               this.showFields(removePupilForm,['rscd_details', 'rscd_reason']);
              break;
         case 'CHANGEYEARGROUP': // reasoncode needed
           this.showFields(removePupilForm, ['rscd_reason', 'rscd_yeargroup']);
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
