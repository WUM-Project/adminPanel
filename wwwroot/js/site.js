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
    // Find the element by ID
    var deleteAttributeButton = document.getElementById('delete_attribute');

    // Check if the element exists before attaching the event listener
    if (deleteAttributeButton) {
        deleteAttributeButton.addEventListener('click', function () {
            removeAttribute(this);
        });
    }
});

function removeAttribute(button) {
    // Видалити поточний елемент списку
    var divWrapper = button.closest('.selected-attribute');
    divWrapper.remove();

    // Оновити приховане поле
    var hiddenAttributesInput = document.getElementById('SelectedAttributes');
    updateHiddenAttributes(hiddenAttributesInput);
}
  
   
    //Робоча версія з select
    // document.getElementById('addAttributeBtn').addEventListener('click', function () {
    //     var select = document.querySelector('select[name="Attributes"]');
    //     var attributeValueInput = document.getElementById('attributeValueInput');
    //     var selectedAttributesList = document.getElementById('selectedAttributesList');
    //     var hiddenAttributesInput = document.getElementById('hiddenAttributes');

    //     // Перевірити, чи є обраний атрибут
    //     if (select.value) {
    //         // Перевірити, чи атрибут вже вибрано
    //         var existingAttribute = document.querySelector('input[value="' + select.value + '"]');
    //         if (existingAttribute) {
    //             // Якщо атрибут вже вибрано, оновити його значення
    //             existingAttribute.nextElementSibling.nextElementSibling.textContent = attributeValueInput.value;

    //             // Оновити значення прихованого поля
    //             existingAttribute.value = select.value + ':' + attributeValueInput.value;

    //             // Перенести елемент вгору (перед першим елементом списку)
    //             selectedAttributesList.insertBefore(existingAttribute.parentElement, selectedAttributesList.firstChild);
    //         } else {
    //             // Якщо атрибут не вибрано, додати його до списку
    //             var divWrapper = document.createElement('div');
    //             divWrapper.className = 'd-flex align-items-center mb-2';

    //             var selectedAttribute = document.createElement('input');
    //             selectedAttribute.type = 'hidden';
    //             selectedAttribute.name = 'Attributes';
    //             selectedAttribute.value = select.value + ':' + attributeValueInput.value;

    //             var selectClone = select.cloneNode(true);
    //             selectClone.value = select.value;
    //             // selectClone.disabled = true;

    //             var textInput = document.createElement('input');
    //             textInput.type = 'text';
    //             textInput.className = 'form-control ms-2 me-2';
    //             textInput.value = attributeValueInput.value;

    //             var deleteButton = document.createElement('button');
    //             deleteButton.type = 'button';
    //             deleteButton.className = 'btn btn-danger';
    //             deleteButton.textContent = 'Delete';
    //             deleteButton.addEventListener('click', function () {
    //                 // Видалити поточний елемент списку
    //                 divWrapper.remove();
    //             });

    //             // Додати елементи до обгортки
    //             divWrapper.appendChild(selectClone);
    //             divWrapper.appendChild(textInput);
    //             divWrapper.appendChild(deleteButton);
    //             divWrapper.appendChild(selectedAttribute);

    //             // Додати обгортку до списку
    //             selectedAttributesList.insertBefore(divWrapper, selectedAttributesList.firstChild);
    //         }

    //         // Скинути значення текстового поля
    //         attributeValueInput.value = '';
    //     }
    // });

    // // Додати обробник події для вибору атрибуту в списку
    // document.querySelector('select[name="Attributes"]').addEventListener('change', function () {
    //     var select = document.querySelector('select[name="Attributes"]');
    //     var attributeValueInput = document.getElementById('attributeValueInput');

    //     // Перевірити, чи атрибут вже вибрано
    //     var existingAttribute = document.querySelector('input[value="' + select.value + '"]');
    //     if (existingAttribute) {
    //         // Якщо атрибут вже вибрано, заповнити значення текстового поля
    //         attributeValueInput.value = existingAttribute.nextElementSibling.nextElementSibling.value;
    //     } else {
    //         // Якщо атрибут не вибрано, скинути значення текстового поля
    //         attributeValueInput.value = '';
    //     }
    // });
    //==================================================================================
    // document.getElementById('addAttributeBtn').addEventListener('click', function () {
    //     var select = document.querySelector('select[name="Attributes"]');
    //     var attributeValueInput = document.getElementById('attributeValueInput');
    //     var selectedAttributesList = document.getElementById('selectedAttributesList');
    //     var hiddenAttributesInput = document.getElementById('hiddenAttributes');

    //     // Перевірити, чи є обраний атрибут
    //     if (select.value) {
    //         // Перевірити, чи атрибут вже вибрано
    //         var existingAttribute = document.querySelector('input[value="' + select.value + '"]');
    //         if (existingAttribute) {
    //             // Якщо атрибут вже вибрано, оновити його значення
    //             existingAttribute.nextElementSibling.nextElementSibling.textContent = attributeValueInput.value;

    //             // Оновити значення прихованого поля
    //             existingAttribute.value = select.value + ':' + attributeValueInput.value;
    //         } else {
    //             // Якщо атрибут не вибрано, додати його до списку
    //             var divWrapper = document.createElement('div');
    //             divWrapper.className = 'd-flex align-items-center mb-2';

    //             var selectedAttribute = document.createElement('input');
    //             selectedAttribute.type = 'hidden';
    //             selectedAttribute.name = 'Attributes';
    //             selectedAttribute.value = select.value + ':' + attributeValueInput.value;

    //             var selectClone = select.cloneNode(true);
    //             selectClone.value = select.value;
    //             // selectClone.disabled = true;

    //             var textInput = document.createElement('input');
    //             textInput.type = 'text';
    //             textInput.className = 'form-control ms-2 me-2';
    //             textInput.value = attributeValueInput.value;

    //             var deleteButton = document.createElement('button');
    //             deleteButton.type = 'button';
    //             deleteButton.className = 'btn btn-danger';
    //             deleteButton.textContent = 'Delete';
    //             deleteButton.addEventListener('click', function () {
    //                 // Видалити поточний елемент списку
    //                 divWrapper.remove();
    //             });

    //             // Додати елементи до обгортки
    //             divWrapper.appendChild(selectClone);
    //             divWrapper.appendChild(textInput);
    //             divWrapper.appendChild(deleteButton);
    //             divWrapper.appendChild(selectedAttribute);

    //             // Вставити новий елемент перед іншими елементами у вибраних атрибутах
    //             selectedAttributesList.insertBefore(divWrapper, selectedAttributesList.firstChild);
    //         }

    //         // Скинути значення текстового поля
    //         attributeValueInput.value = '';
    //     }
    // });

    // // Додати обробник події для вибору атрибуту в списку
    // document.querySelector('select[name="Attributes"]').addEventListener('change', function () {
    //     var select = document.querySelector('select[name="Attributes"]');
    //     var attributeValueInput = document.getElementById('attributeValueInput');

    //     // Перевірити, чи атрибут вже вибрано
    //     var existingAttribute = document.querySelector('input[value="' + select.value + '"]');
    //     if (existingAttribute) {
    //         // Якщо атрибут вже вибрано, заповнити значення текстового поля
    //         attributeValueInput.value = existingAttribute.nextElementSibling.nextElementSibling.value;
    //     } else {
    //         // Якщо атрибут не вибрано, скинути значення текстового поля
    //         attributeValueInput.value = '';
    //     }
    // });

    //=================================================================================
   
   
    /////////////=============================================================================================================

//     $(".marks").click(function (e)
// {
//     var checkBox = $(this).find("input:checkbox:first");
//     if (checkBox.is(":checked"))
//     {
//         checkBox.prop("checked", false);
//     }
//     else
//     {
//         checkBox.prop("checked", true);
//     }
// });
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
        //    $('.categories').change(function () {
        //     var selectedCategories = $('.categories:checked').map(function () {
        //         return $(this).data('id');
        //     }).get();
        //     $('#selectedCategories').val(selectedCategories.join(','));
        // });
     
  
   })
   $(document).ready(function () {
    // $("#fileInput").click(function () {
    //     // Make an AJAX request to initiate the download
    //     $.ajax({
    //         type: "POST",  // Use the appropriate HTTP method (POST in this case)
    //         url: "https://localhost:7144/api/Uploads/SingleUploadImage",  // Replace with the correct controller and action URL
    //         data: { FolderName: "test" },  // Pass any required parameters
    //         success: function (result) {
    //             // Handle the response and initiate the download
    //             console.log(result);
    //             // window.location.href = result.filePath;  // Assuming 'filePath' is the URL of the downloaded file
    //         },
    //         error: function (error) {
    //             console.error("Error:", error);
    //         }
    //     });
    // });
    $("#fileInput").on("change", function () {
        var files = $(this).get(0).files;
        var formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            formData.append(files[i].name, files[i]);
        }
         console.log(formData)
        // uploadFiles(formData);
    })
//      $('#testbtn').click(function () { 
//         console.log("clicked")
//               if (window.FormData !== undefined) { 
//  var fileUpload = $("#FileUpload1").get(0);  
//             var files = fileUpload.files;  
              
//             // Create FormData object  
//             var fileData = new FormData();  
  
//             // Looping over all files and add it to FormData object  
//             for (var i = 0; i < files.length; i++) {  
//                 fileData.append(files[i].name, files[i]);  
//             } 
//              fileData.append('username', "Manas");
//               console.log(fileData);
//                }
//                else {  
//             alert("FormData is not supported.");  
//         }  
//       })
    //   $('#testbtn').click(function () {
    //   var form = new FormData();
    //   var temp = $('#uploadFile').prop('files');
    //   form.append("file",temp[0]);
    //   form.append("FolderName","test");
    //   console.log(form);
    //   uploadFiles(form)
    // })

    
});
function uploadFiles(formData) {
    $.ajax({
        url: "https://localhost:7144/api/Uploads/SingleUploadImage",
        method: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            // var str = "";
            // for (var i = 0; i < data.length; i++) {
            //     str += "<img src='" + data[i] + "' height='100' width='100'>"
            // }

            $("#hiddenId").append(data);
        },
        error: function (data) {
            alert("Upload Failed!");
        }
    })
}
// 'use strict';

// $('.gallery-wrapper').lightGallery({
//     selector: '.gallery-item',
//     subHtmlSelectorRelative: true,
//     prevHtml: '<div class="swiper-button-prev"><i></i></div>',
//     nextHtml: '<div class="swiper-button-next"><i></i></div>'
// });


// if ($('html').find("body").hasClass("lg-on")) {
//     $('html').addClass("overflow-hidden")
// } else {
//     $('html').removeClass("overflow-hidden")
// }
//    /* swiper sliders */
//    _functions.getSwOptions = function(swiper) {
//     let options = swiper.data('options');
//     options = (!options || typeof options !== 'object') ? {} : options;
//     const $p = swiper.closest('.swiper-entry'),
//         slidesLength = swiper.find('>.swiper-wrapper>.swiper-slide').length;
//     if (!options.pagination) options.pagination = {
//         el: $p.find('.swiper-pagination')[0],
//         clickable: true
//     };
//     if (!options.navigation) options.navigation = {
//         nextEl: $p.find('.swiper-button-next')[0],
//         prevEl: $p.find('.swiper-button-prev')[0]
//     };
//     options.preloadImages = false;
//     options.lazy = {
//         loadPrevNext: true
//     };
//     options.observer = true;
//     options.observeParents = true;
//     options.watchOverflow = true;
//     options.centerInsufficientSlides = true;
//     if (!options.speed) options.speed = 500;
//     options.roundLengths = true;
//     if (isTouchScreen) options.direction = "horizontal";
//     if (slidesLength <= 1) {
//         options.loop = false;
//         $p.find('.swiper-wrapper').css({
//             "cursor": "default"
//         })
//     }
//     if (options.customFraction) {
//         $p.addClass('custom-fraction');
//         if (slidesLength > 1 && slidesLength < 10) {
//             $p.find('.custom-current').text('1');
//             $p.find('.custom-total').text(slidesLength);
//         } else if (slidesLength > 1) {
//             $p.find('.custom-current').text('1');
//             $p.find('.custom-total').text(slidesLength);
//         }
//     }
//     return options;
// };
// _functions.initSwiper = function(el) {
//     const swiper = new Swiper(el[0], _functions.getSwOptions(el));
// };

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
})