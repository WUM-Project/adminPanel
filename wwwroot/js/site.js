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
})