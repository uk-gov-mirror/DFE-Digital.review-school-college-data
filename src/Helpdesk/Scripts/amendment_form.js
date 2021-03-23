var Rscd = window.Rscd || {};
(function () {
  const userSettings = Xrm.Utility.getGlobalContext().userSettings;
  const currentUserId = userSettings.userId;
  const currentUserName = userSettings.userName;
  const decision1Field = 'rscd_decision1';
  const decision2Field = 'rscd_decision2';
  const decision3Field = 'rscd_decision3';
  const reviewer1Field = 'rscd_reviewer1';
  const reviewer2Field = 'rscd_reviewer2';
  const reviewer3Field = 'rscd_reviewer3';
  // Code to run in the form OnLoad event
  this.formOnLoad = function (executionContext) {
    const formContext = executionContext.getFormContext();
    const checkingWindowValue = formContext.getAttribute('rscd_checkingwindow').getInitialValue();
    const outcomeReason = formContext.getAttribute('rscd_outcome').getInitialValue();
    //console.log(outcomeReason);
    if (outcomeReason === 501940002) { // awaiting review
       formContext.getControl('rscd_outcome').setVisible(false);
    }
    if (checkingWindowValue == 501940005 || checkingWindowValue == 501940006) { // ks5 checking windows
        formContext.getControl('rscd_uln').setVisible(true); //show ULN field
    } else {
        formContext.getControl('rscd_upn').setVisible(true); //show UPN field
   }
    // if record is inactive, early out
    if (formContext.getAttribute('statecode').getInitialValue() === 1) {
      return;
    }
    this.updateDecisionFields(formContext);
  }
  this.decision1OnChange = function (executionContext) {
    const formContext = executionContext.getFormContext();
    this.setReviewer(formContext, decision1Field, reviewer1Field);
  }
  this.decision2OnChange = function (executionContext) {
    const formContext = executionContext.getFormContext();
    this.setReviewer(formContext, decision2Field, reviewer2Field);
  }
  this.decision3OnChange = function (executionContext) {
    const formContext = executionContext.getFormContext();
    this.setReviewer(formContext, decision3Field, reviewer3Field);
  }
  // Code to run in the form OnSave event
  this.formOnSave = function (executionContext) {
    this.updateDecisionFields(executionContext.getFormContext());
  }
  this.updateDecisionFields = function (formContext) {
    const reviewer1 = formContext.getAttribute(reviewer1Field).getValue()?.[0].id;
    const reviewer2 = formContext.getAttribute(reviewer2Field).getValue()?.[0].id;
    const reviewer3 = formContext.getAttribute(reviewer3Field).getValue()?.[0].id;
    // reset all fields to disabled initially
    formContext.getControl(decision1Field).setDisabled(true);
    formContext.getControl(decision2Field).setDisabled(true);
    formContext.getControl(decision3Field).setDisabled(true);
    if (reviewer1 === currentUserId || reviewer2 === currentUserId || reviewer3 === currentUserId) {
      // current user has already recorded a decision for this record
      return;
    }
    if (formContext.getAttribute(decision1Field).getValue() == null) {
      formContext.getControl(decision1Field).setDisabled(false);
    } else if (formContext.getAttribute(decision2Field).getValue() == null) {
      formContext.getControl(decision2Field).setDisabled(false);
    } else if (formContext.getAttribute(decision3Field).getValue() == null) {
      formContext.getControl(decision3Field).setDisabled(false);
    }
  }
  this.setReviewer = function (formContext, decisionFieldName, reviewerFieldName) {
    const decision = formContext.getAttribute(decisionFieldName).getValue();
    const reviewerField = formContext.getAttribute(reviewerFieldName);
    if (decision == null) {
        // clear associated reviewer field
        reviewerField.setValue(null);
    } else {
      const userValue = new Array();
      userValue[0] = new Object();
      userValue[0].id = currentUserId;
      userValue[0].name = currentUserName;
      userValue[0].entityType = 'systemuser';
      reviewerField.setValue(userValue);
    }
  }
}).call(Rscd);
