﻿const E = window.wangEditor
const editor = new E('#editor')
editor.config.placeholder = 'Please enter the content here'
editor.config.height = 600
editor.config.uploadImgServer = "/File/Upload"
editor.config.uploadImgMaxSize = 2 * 1024 * 1024
editor.config.uploadImgMaxLength = 5
editor.create()