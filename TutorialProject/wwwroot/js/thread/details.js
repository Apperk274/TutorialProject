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
        url: inject("comments-req-url"),
        dataType: "json",
        success: function (comments) {
            for (comment of comments) {
                const commentEl = createCommentEl(comment)
                elCommentsContainer.appendChild(commentEl)
                console.log(commentEl, commentEl.children[0])
                commentEl.setAttribute("data-id", comment.thread.id)
            }
            switchCaretIcon()
            elShowCommentsButton.setAttribute("onclick", "hideComments()")
        },
        error: function (req, status, error) {
            console.log(error);
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
        url: inject("comments-req-url"),
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
            console.log(error)
        },
        complete: function () {
            elAddCommentButton.disabled = false
        }
    });
}

// Util functions

function createCommentEl(comment) {
    const commentEl = templateComment.content.cloneNode(true)
    const activeButtonId = comment.isLiked ? "#likeButton" : comment.isLiked == false ? "#dislikeButton" : null
    if (activeButtonId) commentEl.querySelector(activeButtonId).classList.add("font-weight-bold")
    const likeButtonEl = commentEl.querySelector("#likeButton")
    const dislikeButtonEl = commentEl.querySelector("#dislikeButton")
    likeButtonEl.onclick = function () {
        voteThread(true, comment.thread.id)
    }
    dislikeButtonEl.onclick = function () {
        voteThread(false, comment.thread.id)
    }
    commentEl.querySelector("#title").innerText = comment.thread.title
    commentEl.querySelector("#content").innerText = comment.thread.content
    commentEl.querySelector("#user").innerText = comment.thread.appUser.name
    commentEl.querySelector("#likeCount").innerText = comment.upVotes
    commentEl.querySelector("#dislikeCount").innerText = comment.downVotes
    commentEl.querySelector("#createdAt").innerText = new Date(comment.thread.createdAt).toLocaleString()
    return commentEl
}

function voteThread(isLiked, id) {
    $.ajax({
        type: "POST",
        url: inject("vote-thread-req-url"),
        dataType: "json",
        data: {
            id: id,
            isUp: isLiked,
        },
        success: function () {
            const commentEl = document.querySelector(`#comment[data-id=${id}]`)
            const likeButtonEl = commentEl.querySelector("#likeButton")
            const dislikeButtonEl = commentEl.querySelector("#dislikeButton")
            if (isLiked == true) {
                likeButtonEl.classList.toggle("font-weight-bold")
                dislikeButtonEl.classList.remove("font-weight-bold")
            }
            else {
                dislikeButtonEl.classList.toggle("font-weight-bold")
                likeButtonEl.classList.remove("font-weight-bold")
            }
        },
        error: function (req, status, error) {
            console.log(error)
        },
        complete: function () {
        }
    });
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