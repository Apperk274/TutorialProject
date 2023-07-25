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

function voteThread(isLiked, id, main = false) {
    $.ajax({
        type: "POST",
        url: inject("vote-thread-req-url"),
        dataType: "json",
        data: {
            id: id,
            isUp: isLiked,
        },
        success: function () {
            const boldClass = "font-weight-bold"
            const threadEl = document.querySelector(main ? '#mainThread' : `#comment[data-id="${id}"]`)
            const likeButtonEl = threadEl.querySelector("#likeButton")
            const dislikeButtonEl = threadEl.querySelector("#dislikeButton")
            const likeCountEl = threadEl.querySelector("#likeCount")
            const dislikeCountEl = threadEl.querySelector("#dislikeCount")
            if (isLiked == true) {
                if (likeButtonEl.classList.contains(boldClass)) {
                    likeCountEl.innerText = +likeCountEl.innerText - 1
                } else {
                    likeCountEl.innerText = +likeCountEl.innerText + 1
                }
                if (dislikeButtonEl.classList.contains(boldClass)) {
                    dislikeCountEl.innerText = +dislikeCountEl.innerText - 1
                    dislikeButtonEl.classList.remove(boldClass)
                }
                likeButtonEl.classList.toggle(boldClass)
            }
            else {
                if (dislikeButtonEl.classList.contains(boldClass)) {
                    dislikeCountEl.innerText = +dislikeCountEl.innerText - 1
                } else {
                    dislikeCountEl.innerText = +dislikeCountEl.innerText + 1
                }
                if (likeButtonEl.classList.contains(boldClass)) {
                    likeCountEl.innerText = +likeCountEl.innerText - 1
                    likeButtonEl.classList.remove(boldClass)
                }
                dislikeButtonEl.classList.toggle(boldClass)
            }
        },
        error: function (req, status, error) {
            console.log(error)
        },
        complete: function () {
        }
    });
}

// Util functions

function createCommentEl(comment) {
    const commentEl = templateComment.content.cloneNode(true)
    commentEl.firstElementChild.setAttribute("data-id", comment.thread.id)
    commentEl.querySelector("#title").innerText = comment.thread.title
    commentEl.querySelector("#content").innerText = comment.thread.content
    commentEl.querySelector("#user").innerText = comment.thread.appUser.name
    commentEl.querySelector("#likeCount").innerText = comment.upVotes
    commentEl.querySelector("#dislikeCount").innerText = comment.downVotes
    commentEl.querySelector("#createdAt").innerText = new Date(comment.thread.createdAt).toLocaleString()
    if (isAuthenticated()) {
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
    }
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

function isAuthenticated() {
    return inject("authenticated") === "True"
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