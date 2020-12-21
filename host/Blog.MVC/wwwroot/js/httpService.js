const xsrfFieldName = "XSRF-TOKEN-996BUG-INPUT";
toastr.options = {
    "positionClass": "toast-top-center",
    "timeOut": "1500"
}

function httpDelete({ url, callback = function () { } }) {
    request({ url: url, method: "DELETE", callback: callback });
}

function httpPost({ url, data = {}, callback = function () { } }) {
    request({ url: url, data: data, callback: callback });
}

function httpGet(url) {
    request({ url: url, method: "GET", callback: function () { } });
}

function request({ url, method = "POST", data = {}, callback }) {
    const xsrfValue = $(`input[name=${xsrfFieldName}]`).val();
    axios({
        method: method,
        url: url,
        data: data,
        xsrfCookieName: "XSRF-TOKEN-996BUG",
        headers: { "X-XSRF-TOKEN": xsrfValue }
    }).then(function (response) {
        callback();
        let message = "";
        if (response.data !== null || response.data !== undefined || response.data !== "") {
            message = response.data;
        } else {
            message = "操作成功";
        }
        toastr.success(message);
    }).catch(function (error) {
        toastr.error(error.response.data);
    });
}