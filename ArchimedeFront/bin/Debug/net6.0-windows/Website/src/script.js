const navlinks = document.getElementById("navlinks");
const menu = document.getElementById("menu");

function showmenu() {
    navlinks.style.right = "0";
    // menu.style.display = "none";
    menu.style.opacity= "0"
}
function hidemenu() {
    navlinks.style.right = "-50vw";
    // menu.style.display = "block";
    menu.style.opacity = "1";
}