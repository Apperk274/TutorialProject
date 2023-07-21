let elInjector

let elShowCommentsButton
let elCommentsContainer
let templateComment

function showComments() {
    hideEl(elShowCommentsButton)
    $.ajax({
        type: "GET",
        url: inject("comments-request-url"),
        dataType: "json",
        success: function (comments) {
            for (comment of comments) {
                const commentEl = createCommentEl()
                elCommentsContainer.appendChild(commentEl)
            }
            switchCaretIcon()
            elShowCommentsButton.setAttribute("onclick", "hideComments()")
        },
        error: function (req, status, error) {
            console.log(msg);
        },
        complete: function () {
            showEl(elShowCommentsButton)
        }
    });
}

function hideComments() {
    elCommentsContainer.innerHTML = ""
    elShowCommentsButton.setAttribute("onclick", "showComments()")
    switchCaretIcon()
}

// Util functions

function createCommentEl() {
    const commentEl = templateComment.content.cloneNode(true)
    commentEl.querySelector("#title").innerText = comment.title
    commentEl.querySelector("#content").innerText = comment.content
    commentEl.querySelector("#user").innerText = comment.appUser.name
    commentEl.querySelector("#likeCount").innerText = comment.appUserId
    commentEl.querySelector("#dislikeCount").innerText = comment.appUserId
    commentEl.querySelector("#createdAt").innerText = new Date(comment.createdAt).toLocaleDateString()
    return commentEl
}

function switchCaretIcon() {
    const classUp = "bi-caret-up-square"
    const classDown = "bi-caret-down-square"
    if (elShowCommentsButton.classList.contains(classDown)) {
        elShowCommentsButton.classList.remove(classDown)
        elShowCommentsButton.classList.add(classUp)
    }
    else {
        elShowCommentsButton.classList.remove(classUp)
        elShowCommentsButton.classList.add(classDown)
    }
}

// General functions

function showEl(el) {
    el.classList.remove("d-none")
}
function hideEl(el) {
    el.classList.add("d-none")
}
function inject(name) {
    return elInjector.getAttribute("data-" + name)
}

// Referencing elements

window.onload = function () {
    elShowCommentsButton = document.querySelector("#showCommentsButton")
    templateComment = document.querySelector("template#comment")
    elCommentsContainer = document.querySelector("#commentsContainer")
    elInjector = document.querySelector("#injector")
}