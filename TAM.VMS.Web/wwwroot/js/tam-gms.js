function GenerateGUID() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

function kendoUploadAddPreview(file, wrapper) {
    var raw = file.rawFile;
    var reader = new FileReader();

    if (raw) {
        reader.onloadend = function () {
            var preview = $("<img class='image-preview'>").attr("src", this.result);
            console.log(preview)
            wrapper.find(".k-file[data-uid='" + file.uid + "'] .k-file-extension-wrapper")
                .replaceWith(preview);
        };

        reader.readAsDataURL(raw);
    }
}