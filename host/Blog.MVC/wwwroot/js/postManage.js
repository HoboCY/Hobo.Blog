function deletePost(url, postid) {
    httpDelete({
        url: url,
        callback: function () {
            $(`#tr-${postid}`).hide();
        }
    });
}

function restorePost(url, postid) {
    httpPost({
        url: url,
        callback: function () {
            $(`#tr-${postid}`).hide();
        }
    });
}