/*-----------------------------------------------------------------------------------
/*
/* Main JS
/*
----------------------------------------------------------------------------------- */

(function ($) {


	/* Mobile Menu
   ------------------------------------------------------ */
	var toggle_button = $("<a>", {
		id: "toggle-btn",
		html: "Menu",
		title: "Menu",
		href: "#"
	}
                        );
	var nav_wrap = $('nav#nav-wrap')
	var nav = $("ul#nav");

	/* if JS is enabled, remove the two a.mobile-btns 
  	and dynamically prepend a.toggle-btn to #nav-wrap */
	nav_wrap.find('a.mobile-btn').remove();
	nav_wrap.prepend(toggle_button);

	toggle_button.on("click", function (e) {
		e.preventDefault();
		nav.slideToggle("fast");
	});

	if (toggle_button.is(':visible')) nav.addClass('mobile');
	$(window).resize(function () {
		if (toggle_button.is(':visible')) nav.addClass('mobile');
		else nav.removeClass('mobile');
	});

	$('ul#nav li a').on("click", function () {
		if (nav.hasClass('mobile')) nav.fadeOut('fast');
	});


	/* Smooth Scrolling
  	------------------------------------------------------ */
	$('.smoothscroll').on('click', function (e) {

		e.preventDefault();

		var target = this.hash,
			$target = $(target);

		$('html, body').stop().animate({
			'scrollTop': $target.offset().top
		}, 800, 'swing', function () {
			window.location.hash = target;
		});

	});


	/*	Back To Top Button
	------------------------------------------------------- */
	var pxShow = 300; //height on which the button will show
	var fadeInTime = 400; //how slow/fast you want the button to show
	var fadeOutTime = 400; //how slow/fast you want the button to hide
	var scrollSpeed = 300; //how slow/fast you want the button to scroll to top. can be a value, 'slow', 'normal' or 'fast'

	// Show or hide the sticky footer button
	$(window).scroll(function () {

		if ($(window).scrollTop() >= pxShow) {
			$("#go-top").fadeIn(fadeInTime);
		} else {
			$("#go-top").fadeOut(fadeOutTime);
		}

	});

	//лайки

	$(".likes.clickable").click(function () {
		var likeFielf = $(this);
		var count = likeFielf.find(".likes-counter").html();
		var articleId = likeFielf.attr('article');
		var like = { articleId: articleId };
		$.post(
			'/article/like',
			like,
			null,
			"json").done(function (data) {
				if (data != null && data == true) {
					if (likeFielf.hasClass("checked")) {
						likeFielf.removeClass("checked");
						likeFielf.find(".empty").removeClass("hide").addClass("show");
						likeFielf.find(".full").removeClass("show").addClass("hide");
						likeFielf.find(".likes-counter").html(count - 1);
					} else {
						likeFielf.addClass("checked");
						likeFielf.find(".full").removeClass("hide").addClass("show");
						likeFielf.find(".empty").removeClass("show").addClass("hide");
						likeFielf.find(".likes-counter").html(+count + 1);
					}
				}
			});
	});


	$(".send-comment").click(function (e) {
		var text = $("#commentText").val();
		var articleId = $(e.target).attr('article');
		var comment = { ArticleId: articleId, Text: text };
		$.post(
			'/article/comment',
			comment,
			null,
			"json").done(function (data) {
				if (data != null && data == true) {
					$("#commentText").val("");
					document.location.reload();
				}

			});
	});

	$('#leaveComment').click(function (e) {
		$('#leaveComment').addClass("hide");
		$('.respond').removeClass("hide").addClass("show");
		setTimeout(function () {
			$('#commentText').focus().setCursorPosition();;
		}, 1000);
	});

	$.fn.setCursorPosition = function (pos) {
		this.each(function (index, elem) {
			if (elem.setSelectionRange) {
				elem.setSelectionRange(pos, pos);
			} else if (elem.createTextRange) {
				var range = elem.createTextRange();
				range.collapse(true);
				range.moveEnd('character', pos);
				range.moveStart('character', pos);
				range.select();
			}
		});
		return this;
	};


	$(".custom-check").click((e) => {
		var spans = $(e.target).parent().children("span.glyphicon");
		spans.each((index) => {
			var span = $(spans[index]);
			if (span.hasClass("hide")) {
				span.removeClass("hide");
			} else {
				span.addClass("hide");
			}
		});
	});



	var sortImageList = () => {
		var images = $(".stored-image-wrap");
		images.each((index) => {
			var wrap = $(images[index]);
			var label = $($(".image-preview-wrap-small").parent().find("label")[index]);
			label.html(" Изображение " + (index + 1));
			var inputs = wrap.find("input, textarea");
			inputs.each((iindex) => {
				var input = $(inputs[iindex]);
				var name = input.attr("name");
				if (typeof name != "undefined" && name !== "") {
					console.log(input);
					var newName = "ArticleImages[" + index + "]" + name.substr(name.indexOf("]") + 1, name.length);
					input.attr("name", newName);
					input.attr("id", newName);
				}
			});
		});
	}

	var cleanUpImages = () => {
		var images = $(".stored-image-wrap");
		images.each((index) => {
			var val = $(images[index]).find(".stored-image-id").val();
			if (typeof val == "undefined" || val == null || val <= 0) {
				$(images[index]).remove();
			}
		});
	}

	$(".clean-images-before").click(() => {
		cleanUpImages();
		sortImageList();
	});

	$(".delete-image").click((e) => {
		var image = $(e.target).parents(".aditional-image");
		image.slideUp("slow",
			function () {
				image.remove();
				sortImageList();
			});
	});


	var addPhotoButton = $('.add-photo-button');

	addPhotoButton.click(function () {
		addPhotoToList();

		var number = $('.aditional-image').length;

		$("#setImage" + number).on('click', function (e) {
			var fileInput = $(e.target).attr("fileInputName");
			$("#" + fileInput).click();
		});

		$(".delete-image").last().click((e) => {
			var image = $(e.target).parents(".aditional-image");
			image.slideUp("slow",
				function () {
					image.remove();
					sortImageList();
				});
		});

		$("#ImageInput" + number).on('change', function (e) {
			uploadImage(e);
		});
		$("#setImage" + number).click();

	});

	function addPhotoToList() {
		var number = $('.aditional-image').length + 1;

		var addImg = '<div class="aditional-image stored-image-wrap" id="additionalImage' + number + '"></div>';

		var deledeButton = '<div class="delete-image"><span class="glyphicon glyphicon-remove red"></span></div>';
		var imageWrap = '<div class="form-group col-md-6" id="imageWrap' + number + '"></div>';
		var imgLabel = '<label> Изображение ' + (number) + '</label>';
		var imagePreview = '<div class="image-preview-wrap-small"  id="imagePreview' + number + '"></div>'
		var imageHiddenInput = '<input type="file" class="form-control hide imageInput" imageNumber="' + number + '" postType="photo" id="ImageInput' + number + '" fileName="' + Date.now() + number + '" style="display: none;" />';

		var image = '<img src="/Content/images/articles/no-image.png" style="max-width: 350px; max-height: 197px;" alt="" id="image' + number + '">';
		var changeButton = '<input type="button" id="setImage' + number + '" fileInputName="ImageInput' + number + '" value="Изменить изображение" class="btn btn-default col-md-12 image-button setImage" />';


		var textWrap = '<div class="form-group  col-md-6"  id="textWrap' + number + '"></div>';
		var textLabel = '<label for="ArticleImages[' + number + '].Text"> Описание</label>';
		var textArea = '<textarea class="form-control" name="ArticleImages[' + number + '].Text" id="ArticleImages[' + number + '].Text" style="height: 250px;"></textarea>';

		var imageName = '<input type="text" class="form-control hide" id="ArticleImages[' + number + '].Name" name="ArticleImages[' + number + '].Name" style="display: none;" />';
		var imageType = '<input type="text" class="form-control hide" id="ArticleImages[' + number + '].Type" name="ArticleImages[' + number + '].Type" style="display: none;" />';
		var imageStoredImageId = '<input type="text" class="form-control hide stored-image-id" id="ArticleImages[' + number + '].StoredImageId" name="ArticleImages[' + number + '].StoredImageId" style="display: none;" />';
		var imagePath = '<input type="text" class="form-control hide" id="ArticleImages[' + number + '].Path" name="ArticleImages[' + number + '].Path" style="display: none;" />';
		var imageExtention = '<input type="text" class="form-control hide" id="ArticleImages[' + number + '].Extention" name="ArticleImages[' + number + '].Extention" style="display: none;" />';
		var imageSizes = '<input type="text" class="form-control hide" id="ArticleImages[' + number + '].Sizes" name="ArticleImages[' + number + '].Sizes" style="display: none;" />';



		addPhotoButton.before(addImg);

		$("#additionalImage" + number).append(imageWrap, textWrap, imageName, imageType, imageStoredImageId, imagePath, imageExtention, imageSizes);
		$("#imageWrap" + number).append(deledeButton, imgLabel, imagePreview, imageHiddenInput);
		$("#imagePreview" + number).append(image, changeButton);
		$("#textWrap" + number).append(textLabel, textArea);
	}


	// *******************  показать еще **********************
	var showButton = $("#show-comments-button");
	var commentsLeft = $("#comments-left");
	var commentsShown = $("#comments-shown");

	var comments = $("li.comments-item");

	if (comments.length > 5) {

		comments.addClass("hide");

		comments.each((index) => {
			if (index >= comments.length - 5) {
				$(comments[index]).toggleClass("hide");
				$(comments[index]).toggleClass("show");
			}
		});
		comments = $("li.comments-item.hide");
		if (comments.length < 5)
			commentsLeft.html(comments.length);
		var sc = ($("li.comments-item.show"));
		commentsShown.html(sc.length + " из ");

		showButton.click(() => {

			var comments = $("li.comments-item.hide");

			var lastVisibleComment = $($("li.comments-item.show")[0]);

			if (comments.length <= 5) {
				comments.toggleClass("hide");
				comments.toggleClass("show");
				showButton.toggleClass("hide");
				commentsShown.toggleClass("hide");
			} else {
				comments.each((index) => {
					if (index >= comments.length - 5) {
						$(comments[index]).toggleClass("hide");
						$(comments[index]).toggleClass("show");
					}
				});
				comments = $("li.comments-item.hide");
				if (comments.length < 5)
					commentsLeft.html(comments.length);
			}

			var sc = ($("li.comments-item.show"));
			commentsShown.html(sc.length + " из ");

			$('html, body').animate({
				scrollTop: (lastVisibleComment.offset().top - 400)
			},
				500);


		});

	} else {
		showButton.addClass("hide");
		commentsShown.addClass("hide");
	}
	// *******************  показать еще  **********************


	// ******************* cookies ************************ //

	$("#cookies").slideDown();

	$("#accept-cookies").click(() => {
		setCookie(".PECOOKIEACCEPTED", Date.now(), { expires: 1000*36000 });
		$("#cookies").slideUp();
	});

	function setCookie(name, value, options) {
		options = options || {};

		var expires = options.expires;

		if (typeof expires == "number" && expires) {
			var d = new Date();
			d.setTime(d.getTime() + expires * 1000);
			expires = options.expires = d;
		}
		if (expires && expires.toUTCString) {
			options.expires = expires.toUTCString();
		}

		value = encodeURIComponent(value);

		var updatedCookie = name + "=" + value;

		for (var propName in options) {
			updatedCookie += "; " + propName;
			var propValue = options[propName];
			if (propValue !== true) {
				updatedCookie += "=" + propValue;
			}
		}

		document.cookie = updatedCookie;
	}
})(jQuery);