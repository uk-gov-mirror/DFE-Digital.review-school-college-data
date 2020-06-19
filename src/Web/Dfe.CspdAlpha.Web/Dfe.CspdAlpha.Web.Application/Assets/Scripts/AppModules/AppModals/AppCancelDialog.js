import AppModal from './AppModal';

class AppCancelDialog extends AppModal{
  constructor(el, opts) {
    super(el, opts);
    this.bindButtonEvents();
  }

  bindButtonEvents() {
    $(document).on('click','.app-modal__button-positive', (e)=>{
      e.preventDefault();
      window.location = this.openedBy.getAttribute('href');
    });

    $(document).on('click','.app-modal__button-negative',(e)=> {
      e.preventDefault();
      this.closeModal();
    });
  }
}


export default AppCancelDialog;
