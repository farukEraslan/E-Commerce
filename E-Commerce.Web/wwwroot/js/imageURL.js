function previewFile() {
    const preview = document.getElementById("productImage");
    const imageUrl = document.getElementById("imageUrl");
    const file = document.getElementById("imageInput").files[0];
    const reader = new FileReader();

    reader.addEventListener("load", () => {
        // convert image file to base64 string
        if (file.type === "image/png" || file.type === "image/jpg" || file.type === "image/jpeg") {
            preview.src = reader.result;
            imageUrl.value = reader.result;
            console.log(imageUrl.value);
        }
    }, false,
    );

    if (file) {
        reader.readAsDataURL(file);
    }
}