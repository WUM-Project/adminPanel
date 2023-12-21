// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(function($) {
    //Версія з dropDown list 
    // Змінити значення dropdown та прихованого поля
document.querySelectorAll('.dropdown-item:not(.selected)').forEach(function (item) {
    item.addEventListener('click', function (e) {
        e.preventDefault();
        var attributeDropdown = document.getElementById('attributeDropdown');
        var attributeValueInput = document.getElementById('attributeValueInput');
        if (attributeDropdown) {
            // Змінити значення dropdown та прихованого поля
            attributeDropdown.textContent = this.textContent;
            attributeDropdown.dataset.value = this.dataset.value;

            // Скинути значення текстового поля
            attributeValueInput.value = '';
        }
    });
});
var attributeBtn = document.getElementById('addAttributeBtn');
if(attributeBtn){
document.getElementById('addAttributeBtn').addEventListener('click', function (e) {
    var attributeDropdown = document.getElementById('attributeDropdown');
    var attributeValueInput = document.getElementById('attributeValueInput');
    var selectedAttributesList = document.getElementById('selectedAttributesList');
    var hiddenAttributesInput = document.getElementById('SelectedAttributes');
    e.preventDefault();

    // Перевірити, чи є обраний атрибут
    if (attributeDropdown.dataset.value) {
        // Перевірити, чи атрибут вже вибрано
        var isAlreadySelected = selectedAttributesList.querySelector('.selected-attribute[data-value="' + attributeDropdown.dataset.value + '"]');
        if (!isAlreadySelected) {
            // Додати атрибут до списку
            var divWrapper = document.createElement('div');
            divWrapper.className = 'd-flex align-items-center mb-2 selected-attribute';
            divWrapper.dataset.value = attributeDropdown.dataset.value;

            var selectedAttribute = document.createElement('input');
            selectedAttribute.type = 'hidden';
            selectedAttribute.className = 'attribute-input';
            selectedAttribute.name = 'Attributes';
            selectedAttribute.value = attributeDropdown.dataset.value + ':' + attributeValueInput.value;

            var dropdownClone = attributeDropdown.cloneNode(true);
            dropdownClone.textContent = attributeDropdown.textContent;
            dropdownClone.dataset.value = attributeDropdown.dataset.value;

            var textInput = document.createElement('input');
            textInput.type = 'text';
            textInput.className = 'form-control ms-2 me-2 attribute-value';
            textInput.value = attributeValueInput.value;

            var deleteButton = document.createElement('button');
            deleteButton.type = 'button';
            deleteButton.className = 'btn btn-danger';
            deleteButton.textContent = 'Delete';
            deleteButton.addEventListener('click', function () {
                // Видалити поточний елемент списку
                console.log()
                divWrapper.remove();
                // Зняти заборону для випадаючого списку
                dropdownClone.removeAttribute('disabled');
            });

            // Додати елементи до обгортки
            divWrapper.appendChild(dropdownClone);
            divWrapper.appendChild(textInput);
            divWrapper.appendChild(deleteButton);
            divWrapper.appendChild(selectedAttribute);

            // Заборонити випадаючий список для обраного значення
            dropdownClone.setAttribute('disabled', 'disabled');

            // Додати обгортку до списку
            selectedAttributesList.insertBefore(divWrapper, selectedAttributesList.firstChild);

            // Скинути значення текстового поля
            attributeValueInput.value = '';

             // Оновити приховане поле
             updateHiddenAttributes(hiddenAttributesInput);
        }
    }
});

// Допоміжна функція для оновлення прихованого поля Attributes
function updateHiddenAttributes(hiddenAttributesInput) {
    var selectedAttributes = document.querySelectorAll('.selected-attribute .attribute-input');
    var attributeValues = Array.from(selectedAttributes).map(function (attribute) {
        return attribute.value;
    });
    hiddenAttributesInput.value = attributeValues.join(',');
    // hiddenAttributesInput.value = attributeValues.join(',');
    // hiddenAttributesInput.value = JSON.stringify(attributeValues);

    console.log("Updated Hidden Attributes:", hiddenAttributesInput.value);
}

$(document).ready(function () {
    var deleteAttributeButtons = document.querySelectorAll('.delete-attribute');

    // Attach the event listener to each 'Delete' button
    deleteAttributeButtons.forEach(function(button) {
        button.addEventListener('click', function () {
            console.log("clicked");
            removeAttribute(this);
        });
    });
    // // Find the element by ID
    // var deleteAttributeButton = document.getElementById('delete_attribute');
    //      console.log("worked")
         
    // // Check if the element exists before attaching the event listener
    // if (deleteAttributeButton) {
    //     deleteAttributeButton.addEventListener('click', function () {
    //         console.log("clicked");
    //         removeAttribute(this);
    //     });
    // }
});

function removeAttribute(button) {
    // Видалити поточний елемент списку
    var divWrapper = button.closest('.selected-attribute');
    divWrapper.remove();

    // Оновити приховане поле
    var hiddenAttributesInput = document.getElementById('SelectedAttributes');
    updateHiddenAttributes(hiddenAttributesInput);
}
  
} 

   
    $(document).on('click', '.btn.create-product', async function () {
       let error
    
       let data = {}
       
       //For get check
       let selectedMarks = [];
       let selectedCategories = [];
       let selectedAttributesValue=[];
       $('.location-popup-radios .checkbox-entry').each(function (index, value) {
           if ($(this).find('input').is(':checked')) {
               selectedMarks.push($(this).attr('data-id'));
           }
       });
           $('#selectedMarks').val(selectedMarks.join(','));


           $('.location-popup-categories .checkbox-entry').each(function (index, value) {
            if ($(this).find('input').is(':checked')) {
                selectedCategories.push($(this).attr('data-id'));
            }
        });
            $('#selectedCategories').val(selectedCategories.join(','));
       
            // var selectedBrand = $('#brandDropdown').val();
            $('#SelectedBrand').val( $('#brandDropdown').val());
  
   })

   $(document).ready(function () {
    $('#brandDropdown').on('change', function () {
        var selectedBrand = $(this).val();
        $('#SelectedBrand').val(selectedBrand);
    });
   
    $("#fileInput").on("change", function () {
        var files = $(this).get(0).files;
        var formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            formData.append(files[i].name, files[i]);
        }
         console.log(formData)
        // uploadFiles(formData);
    })


    
});


  // Ініціалізація Swiper після завантаження документа
  document.addEventListener('DOMContentLoaded', function () {
    var mySwiper = new Swiper('.swiper-container', {
        // Опції Swiper тут
        loop: true, // якщо ви хочете їхати по колу
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
    });
});




//====================Нове завантаження картинок=======================

$(document).on('click', '.delete-button', async function (e) {
    e.preventDefault();
    // Отримати батьківський контейнер зображення
    var imageContainer = $(this).closest('.image-container');
    var fileId = $(this).data('file-id');
    
    // Виклик серверного методу для видалення файлу за ідентифікатором
    try {
        if(fileId){
        await deleteFile(fileId);

         // Забрати ідентифікатор з рядка значень прихованого поля
         var currentIds = $('#uploadedImageIds').val();
         if(currentIds){
         var idsArray = currentIds.split(',');
         var indexToRemove = idsArray.indexOf(fileId.toString());
         if (indexToRemove !== -1) {
             idsArray.splice(indexToRemove, 1);
             // Оновити значення прихованого поля без видаленого ідентифікатора
             $('#uploadedImageIds').val(idsArray.join(','));
         }
        }
    }
        // Видалити батьківський контейнер зображення з гріду
        imageContainer.remove();
    } catch (error) {
        console.error('Помилка при видаленні файлу на сервері:', error);
    }
});

// Функція для видалення файлу на сервері за ідентифікатором
async function deleteFile(fileId) {
    console.log(fileId);
    // Здійснюємо Ajax-запит для виклику серверного методу видалення
    await $.ajax({
        type: 'DELETE',
        url: 'https://localhost:7144/api/Uploads/' + fileId, // Замініть на свій URL
        contentType: 'application/json',
        success: function () {
            console.log('Файл успішно видалено на сервері.');
        },
        error: function (xhr, status, error) {
            console.error('Помилка при видаленні файлу на сервері:', error);
            throw new Error('Помилка при видаленні файлу на сервері.');
        }
    });
}
$(document).ready(function () {

    //  // Обробник події при зміні вибраних файлів
     $('#brandImage').on('change', function (e) {
        var files = e.target.files;
           console.log("ddddsfsss");
        // Створюємо об'єкт FormData для відправлення файлів
        var formData = new FormData();

        // Додаємо файли до об'єкту FormData
        for (var i = 0; i < files.length; i++) {
            formData.append('file', files[i]); }
    
        // Додаємо додаткові дані (якщо потрібно)
        formData.append('FolderName', 'brands');
    
        // Здійснюємо Ajax-запит для завантаження файлів на сервер
        $.ajax({
            type: 'POST',
            url: 'https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=brands',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
           if(response && response.id)
                      $('#previewBrandId').val(response.id);
                 
                      $('#previewImg').append(
                        '<div class="image-container">' +
                        '<img src="' + response.filePath + '" alt="Image">' +
                        '<button class="delete-button" data-file-id="' + response.id + '">Видалити</button>' +
                        '</div>'
                    );
               
            },
            error: function () {
                console.error('Помилка при завантаженні файлів на сервер.');
            }
        });
    });
    // Масив для зберігання ідентифікаторів завантажених файлів
    // var uploadedImageIdsArray = [];
    
var currentUploadedIds = $('#uploadedImageIds').val();

   

    // Обробник події при зміні вибраних файлів
    $('#galleryFiles').on('change', function (e) {
        var files = e.target.files;
   
        // Створюємо об'єкт FormData для відправлення файлів
        var formData = new FormData();

        // Додаємо файли до об'єкту FormData
        for (var i = 0; i < files.length; i++) {
            formData.append('file', files[i]); }
    
        // Додаємо додаткові дані (якщо потрібно)
        formData.append('FolderName', 'test');
    
        // Здійснюємо Ajax-запит для завантаження файлів на сервер
        $.ajax({
            type: 'POST',
            url: 'https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=productGallery',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
              // Оновлюємо скритий інпут з ідентифікаторами
var currentUploadedIds = $('#uploadedImageIds').val();



              
                // // Додаємо ідентифікатори файлів до масиву
                // uploadedImageIdsArray.push(response.id);
                // // Оновлюємо скритий інпут з ідентифікаторами
                // $('#uploadedImageIds').val(uploadedImageIdsArray.join(','));

                // Оновлюємо грід з новими файлами
                if(response && response.filePath){
                    currentUploadedIds += (currentUploadedIds.length > 0 ? ',' : '') + response.id;


                    // Оновлюємо скритий інпут з ідентифікаторами
                      $('#uploadedImageIds').val(currentUploadedIds);
                     updateGridView(response.filePath,response.id);
                }
               
            },
            error: function () {
                console.error('Помилка при завантаженні файлів на сервер.');
            }
        });
    });

    // Функція для оновлення гріда
    function updateGridView(filePath,fileId) {
      
            $('#imageGridWrapper').append(
                '<div class="image-container">' +
                '<img src="' + filePath + '" alt="Image">' +
                '<button class="delete-button" data-file-id="' + fileId + '">Видалити</button>' +
                '</div>'
            );
        
    }
    // Обробник події при зміні вибраних файлів
    $('#uploadFile').on('change', function (e) {
        var files = e.target.files;
   
        // Створюємо об'єкт FormData для відправлення файлів
        var formData = new FormData();

        // Додаємо файли до об'єкту FormData
        for (var i = 0; i < files.length; i++) {
            formData.append('file', files[i]); }
    
        // Додаємо додаткові дані (якщо потрібно)
        formData.append('FolderName', 'product');
    
        // Здійснюємо Ajax-запит для завантаження файлів на сервер
        $.ajax({
            type: 'POST',
            url: 'https://localhost:7144/api/Uploads/SingleUploadImage?FolderName=productviews',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
           if(response && response.id)
                      $('#previewImageId').val(response.id);
                 
                      $('#previewImg').append(
                        '<div class="image-container">' +
                        '<img src="' + response.filePath + '" alt="Image">' +
                        '<button class="delete-button" data-file-id="' + response.id + '">Видалити</button>' +
                        '</div>'
                    );
               
            },
            error: function () {
                console.error('Помилка при завантаженні файлів на сервер.');
            }
        });
    });
   

  
});



$(document).on('click', '.btn.edit-product', async function () {
    let error
 
    let data = {}
    
    //For get check
    let selectedMarks = [];
    let selectedCategories = [];
    let selectedAttributesValue=[];
    $('.location-popup-radios .checkbox-entry').each(function (index, value) {
        if ($(this).find('input').is(':checked')) {
            selectedMarks.push($(this).attr('data-id'));
        }
    });
        $('#selectedMarks').val(selectedMarks.join(','));

       console.log(selectedMarks);
        $('.location-popup-categories .checkbox-entry').each(function (index, value) {
         if ($(this).find('input').is(':checked')) {
             selectedCategories.push($(this).attr('data-id'));
         }
     });
         $('#selectedCategories').val(selectedCategories.join(','));
         $('#SelectedBrand').val( $('#brandDropdown').val());
   

})


})