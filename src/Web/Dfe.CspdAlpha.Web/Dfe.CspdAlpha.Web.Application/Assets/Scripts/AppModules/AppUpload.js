class AppUpload {
  constructor() {
    this.init();
  }

  init() {
    this.uploadControl = $('<div class="govuk-form-group app-upload__element"><label class="govuk-label" for="EvidenceFiles">Upload file</label><input class="govuk-file-upload" type="file" id="EvidenceFiles" name="EvidenceFiles"></div>');
    this.fileNames = [];
    const uploadBtn = document.getElementById('upload-button');

    uploadBtn.addEventListener('click', (e) =>{
      let currentUpload = $('.govuk-file-upload').filter(':visible')[0].files[0];
      if (currentUpload) {
        e.preventDefault();
        $('.app-upload__element').filter(':visible').data('fName', currentUpload.name).addClass('hidden');

        $('#app-upload__files-container').prepend(this.uploadControl.clone());
        this.fileNames.push(currentUpload.name);
        this.updateFileList();
      } else {

        //$('#upload-button').next('dot-pulse').removeClass('hidden');
      }
    });
  }

  removeFileFromCollection(fileName) {
    const index = this.fileNames.indexOf(fileName);
    this.fileNames.splice(index, 1);

    $('.app-upload__element').filter((n,el)=> {
      return $(el).data().fName === fileName;
    }).remove();

    this.updateFileList();
  }

  updateFileList() {
    const fileList = document.getElementById('upload-file-list');

    fileList.classList.remove('hidden');
    fileList.innerHTML = '';
    let frag = document.createDocumentFragment();
    const li = document.createElement('li');
    const button = document.createElement('input');
    button.type = 'button';
    button.value = 'remove file';
    button.className = 'app-upload__remove';
    const buttonText = document.createTextNode('remove');
    button.appendChild(buttonText);

    let listHeading = li.cloneNode();
    let heading = document.createElement('h2');
    heading.className = 'govuk-heading-s';
    const headingText = document.createTextNode('You added');

    heading.appendChild(headingText);
    listHeading.appendChild(heading);
    frag.appendChild(listHeading);

    this.fileNames.forEach((fileName)=> {
      const _li = li.cloneNode();
      const text =  document.createTextNode(fileName);
      const _btn = button.cloneNode();
      _btn.addEventListener('click', (e)=>{
        e.preventDefault();
        this.removeFileFromCollection(fileName);
      });
      _li.appendChild(text);
      _li.appendChild(_btn);

      frag.appendChild(_li);
    });

    fileList.appendChild(frag);
    if (this.fileNames.length === 0) {
      fileList.classList.add('hidden');
    }
  }
}

export default AppUpload;
