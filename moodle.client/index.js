function loginAction() {
    var username = document.getElementById('usernameInput').value;
	localStorage.setItem('username', username);
    window.location.href = "mainPage.html";
}

