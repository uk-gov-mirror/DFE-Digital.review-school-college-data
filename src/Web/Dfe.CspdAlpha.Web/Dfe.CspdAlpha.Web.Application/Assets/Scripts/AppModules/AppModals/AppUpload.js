class AppUpload {
  constructor() {
    this.init();
  }

  init() {
    this.formData = new FormData(document.getElementById('upload-form'));
    this.formData.delete('EvidenceFiles');
    this.fileNames = [];
    const uploadBtn = document.getElementById('upload-button');
    const uploadField = document.getElementById('EvidenceFiles');
    let fileCount = 0;

    /* eslint no-console: 0 */
    uploadBtn.addEventListener('click', (e) =>{
      e.preventDefault();
      let currentUpload = uploadField.files[0];
      if (currentUpload) {
        this.formData.append('EvidenceFiles', currentUpload);
        this.fileNames.push(currentUpload.name);
        fileCount ++;
      }
      else if (fileCount > 0) {
        $.ajax({
          url: window.location,
          method: 'post',
          data: this.formData,
          processData: false,
          contentType: false,
          success: function() {
            const punt = window.location.toString().replace('/UploadEvidence', '/Inclusion');
            window.location = punt;
          },
          error: function(err) {
            console.log(err);
          }
        });
      }
      this.updateFileList();
      uploadField.value = '';
    });
  }

  removeFileFromCollection(fileName) {
    const index = this.fileNames.indexOf(fileName);
    this.fileNames.splice(index, 1);

    const currentFiles = this.formData.getAll('EvidenceFiles');
    this.formData.delete('EvidenceFiles');

    for (let i = 0, len = currentFiles.length; i < len; i++) {
      if (currentFiles[i].name !== fileName) {
        this.formData.append('EvidenceFiles', currentFiles[i]);
      }
    }

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
