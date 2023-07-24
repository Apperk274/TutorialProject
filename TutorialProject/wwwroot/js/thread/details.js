let elInjector

let elShowCommentsButton
let elCommentsContainer
let elNewCommentContentInput
let elNewCommentTitleInput
let elAddCommentButton
let templateComment

function showComments() {
    hideEl(elShowCommentsButton)
    $.ajax({
        type: "GET",
        url: inject("comments-request-url"),
        dataType: "json",
        success: function (comments) {
            for (comment of comments) {
                const commentEl = createCommentEl(comment)
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

function addComment() {
    elAddCommentButton.disabled = true
    const title = elNewCommentTitleInput.value
    const content = elNewCommentContentInput.value
    $.ajax({
        type: "POST",
        url: inject("comments-request-url"),
        dataType: "json",
        data: {
            title: title,
            content: content,
            parentId: inject("thread-id")
        },
        success: function (newComment) {
            const newCommentEl = createCommentEl(newComment)
            elCommentsContainer.insertBefore(newCommentEl, elCommentsContainer.firstChild)
        },
        error: function (req, status, error) {
            console.log(msg)
        },
        complete: function () {
            elAddCommentButton.disabled = false
        }
    });
}

// Util functions

function createCommentEl(comment) {
    const commentEl = templateComment.content.cloneNode(true)
    commentEl.querySelector("#title").innerText = comment.title
    commentEl.querySelector("#content").innerText = comment.content
    commentEl.querySelector("#user").innerText = comment.appUser.name
    commentEl.querySelector("#likeCount").innerText = comment.appUserId
    commentEl.querySelector("#dislikeCount").innerText = comment.appUserId
    commentEl.querySelector("#createdAt").innerText = new Date(comment.createdAt).toLocaleString()
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
    elNewCommentTitleInput = document.querySelector("#newCommentTitleInput")
    elNewCommentContentInput = document.querySelector("#newCommentContentInput")
    elAddCommentButton = document.querySelector("#addCommentButton")
    elInjector = document.querySelector("#injector")
}