document.querySelectorAll(".form-select").forEach(el => {
    const param = new URLSearchParams(window.location.search).get(el.id);
    el.value = param ?? "all";
    el.addEventListener("change", function () {
        const url = new URL(window.location);
        if (this.value == "all") url.searchParams.delete(el.id);
        else url.searchParams.set(el.id, this.value);
        window.location = url;
    });
});
const search = document.getElementById("s");
const param = new URLSearchParams(window.location.search).get("s");
search.value = param ?? "";
document.getElementById("search").addEventListener("submit", function (e) {
    e.preventDefault();
    const url = new URL(window.location);
    if (!search.value) url.searchParams.delete("s");
    else url.searchParams.set("s", search.value);
    window.location = url;
});