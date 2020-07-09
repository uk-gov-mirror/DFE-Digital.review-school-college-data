import AppModal from './AppModal';
/* eslint no-console: 0 */
class AppCancelDialog extends AppModal{
  constructor(el, opts) {
    super(el, opts);

    $(window).on('modal:opened', (e)=> {
      if (e.element === this.el) {
        this.bindButtonEvents();
      }
    });

    $(window).on('modal:closed', ()=> {
      this.unbindButtonEvents();
    });
  }

  bindButtonEvents() {
    console.log('bind buttons ' , this.el);
    $('.app-modal__button-positive').on('click', (e)=>{
      e.preventDefault();
      console.log('YES click: ', this.el);
      $('#app-modal-close').click();
      window.location = this.el.getAttribute('href');
    });

    $('.app-modal__button-negative').on('click',(e)=> {
      e.preventDefault();
      $('#app-modal-close').click();
    });
  }

  unbindButtonEvents() {
    console.log('unbind buttons');
    $('.app-modal__button-positive').off('click');
    $('.app-modal__button-negative').off('click');
  }
}


export default AppCancelDialog;
