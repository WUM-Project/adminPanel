jQuery(function($) {
    $(document).on('click', '.btn.create-product', async function () {
       let error
    
       let data = {}
       let form = $(this).parent().parent();
    
   
       data.Marks = []
       $('.location-popup-radios .checkbox-entry').each(function (index, value) {
           if ($(this).attr('data-type')) {
               if ($(this).find('input').is(':checked')) {
                   data.Marks.push({
                       id: $(this).attr('data-id'),
                     
                       title: $(this).find('span').text(),
                   })
               }
           }
       })
         console.log(data.Marks)
     alert(data.Marks)
      console.log("Shlyapa")
     
     
    //    formData.append('data', JSON.stringify(data))
    
           $.ajax({
               type: "post",
               url: '/Products/Create',
               data: data,
               enctype: 'multipart/form-data',
               async: true,
               cache: false,
               processData: false,
               contentType: false,
               dataType: "json",
               success: function (data) {
                 console.log(formData)

               },
               error: function (data) {
                   console.log(data.responseJSON.message)
               }
           });
  
       return false
   })
})