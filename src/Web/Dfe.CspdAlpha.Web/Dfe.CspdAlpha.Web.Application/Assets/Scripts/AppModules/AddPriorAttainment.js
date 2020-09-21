class AddPriorAttainment {
  constructor() {
    this.init();
  }

  init() {

    const radioContainer = document.getElementById('subject-selection');
    const scoreLabel = document.getElementById('test-mark-label');

    const radios = $(radioContainer).find('.govuk-radios__input');

    radios.on('change', function() {
      scoreLabel.innerText = this.value === 'Writing' ? 'Teacher assessment' : 'Test mark';
    });

  }
}

export default AddPriorAttainment;
