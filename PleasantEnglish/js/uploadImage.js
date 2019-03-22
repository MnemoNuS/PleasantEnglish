$('.setImage').on('click', function (e) {
	var fileInput = $(e.target).attr("fileInputName");
	$("#"+fileInput).click();
});

$('.imageInput').on('change', function (e) {
	uploadImage(e);
});

function uploadImage (e) {
	
	var files = e.target.files;
	var fileName = $(e.target).attr("fileName");
	var postType = $(e.target).attr("postType");
	var imageNumber = $(e.target).attr("imageNumber");

	if (files.length > 0) {
		if (window.FormData !== undefined) {
			var data = new FormData();
			for (var x = 0; x < files.length; x++) {
				data.append("file" + x, files[x]);
			}
			var url = '/Editor/UploadFile?imageNumber=' + imageNumber + '&fileName=' + fileName;
			if (postType == "photo")
				url += "&photoPost=true";
			$.ajax({
				type: "POST",
				url: url,
				contentType: false,
				processData: false,
				data: data,
				success: function (result) {
					console.log(result);
					var storedFile = result.storedFile;
					$('#image' + imageNumber).attr("src", "/Content/images/articles/" + storedFile.Name + storedFile.Extention + ".[small]" + storedFile.Extention + "?timestamp=" + new Date().getTime());
					$('input[name*="ArticleImages[' + imageNumber + '].StoredImageId"]').val(storedFile.StoredImageId);
					$('input[name*="ArticleImages[' + imageNumber + '].Type"]').val(storedFile.Type);
					$('input[name*="ArticleImages[' + imageNumber + '].Path"]').val(storedFile.Path);
					$('input[name*="ArticleImages[' + imageNumber + '].Name"]').val(storedFile.Name);
					$('input[name*="ArticleImages[' + imageNumber + '].Extention"]').val(storedFile.Extention);
					$('input[name*="ArticleImages[' + imageNumber + '].Sizes"]').val(storedFile.Sizes);
				},
				error: function (xhr, status, p3, p4) {
					var err = "Error " + " " + status + " " + p3 + " " + p4;
					if (xhr.responseText && xhr.responseText[0] == "{")
						err = JSON.parse(xhr.responseText).Message;
					console.log(err);
				}
			});
		} else {
			alert("This browser doesn't support HTML5 file uploads!");
		}
	}
}