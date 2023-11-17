// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(function($) {
   

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

})