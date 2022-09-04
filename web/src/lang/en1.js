export default {
  // 常用按钮
  baseBtn: {
    importBtn: 'Import', // 导入按钮
    addBtn: 'Add', // 添加
    queryBtn: 'Query', // 查询
    templateBtn: 'Template', // 下载模板
    exportBtn: 'Export', // 导出
    exportBtnWhole: 'Export all', // 全部导出
    exportBtnCondition: 'Conditional export', // 条件导出
    editBtn: 'Edit', // 编辑
    removeBtn: 'Remove', // 移除
    Operation: 'Operation', // 操作

    cancel: 'Cancel', // 取消
    confirm: 'Confirm', // 确认
    SelectPicture: 'Select Picture' // 选择图片

  },
  // 常见提示文字
  messageTips: {
    // 创建
    messageSucceed: 'Created successfully', // 创建成功
    messageFailure: 'Creation failed', // 创建失败
    // 编辑
    editSucceed: 'Edit succeeded', // 编辑成功
    editFailure: 'Edit failed', // 编辑失败
    // 删除
    deleteTips: 'Prompt that this operation will permanently delete the data. Do you want to continue?', // 删除提示:此操作将永久删除该数据, 是否继续?
    Tips: 'Tips', // 提示
    deleteSucceed: 'Deletion succeeded', // 删除成功
    deleteFailure: 'Deletion failed', // 删除失败
    messageCancel: 'Cancelled', // 已取消
    // 导入
    ImportSucceed: 'Import succeeded', // 导入成功
    ImportFailure: 'Import failed', // 导入失败
    ImportJudge: 'Currently, only one file can be selected. Please delete it and continue to upload。', // 当前限制选择 1 个文件，请删除后继续上传

    // 导出
    Export: 'You are about to export all of your data. Do you want to continue?', // 您将导出所有的所有的数据是否继续?

    Succeed: 'Succeed', // 成功
    Failure: 'Failure' // 失败
  },
  navbar: {
    dashboard: 'Dashboard',
    github: 'Github',
    logOut: 'Log Out',
    profile: 'Profile',
    theme: 'Theme',
    size: 'Global Size'
  },
  login: {
    title: 'Langitec-IWMS',
    logIn: 'Login',
    username: 'Username',
    password: 'Password',
    any: 'any',
    thirdparty: 'Or connect with',
    thirdpartyTips: 'Can not be simulated on local, so please combine you own business simulation! ! !'
  },
  // 载具管理
  box: {
    // 查询栏输入提示数据
    queryBack: 'Box Code & Box Name',
    // 表头
    Code: 'BoxCode',
    Picture: 'Picture',
    Colour: 'Colour',
    Name: 'Name',
    Type: 'Type',
    width: 'width',
    length: 'length',
    IsVirtual: 'virtual',
    Introduce: 'Virtual vehicle',
    CreatedUserName: 'Added by',
    CreatedTime: 'Add time',

    // 创建编辑
    update: 'Edit vehicle box',
    create: 'Create vehicle box',

    // 创建输入框提示
    inputBoxCode: 'Please enter the vehicle box name', // 输入载具箱编码
    inputBoxName: 'Please enter the vehicle box name', // 输入载具箱名称
    inputBoxlength: 'Please input the length of vehicle box', // 输入载具箱长度
    inputBoxwidth: 'Please input the width of vehicle box', // 输入载具箱宽度
    inputBoxType: 'Please select type', // 请选择种类
    inputBoxColour: 'Please choose a color', // 请选择颜色
    inputBoxRemarks: 'Introduce' // 备注
  },
  // 设备型号
  EquipmentType: {
    // 设备型号表头
    Code: 'EquipmentCode',
    Picture: 'Picture',
    Remark: 'Describe',
    brand: 'Brand',
    Type: 'Type',
    CreatedUserName: 'Added by',
    CreatedTime: 'Add time',

    // 页面查找输入框提示
    choiceEquipmentCode: 'Please select the equipment model', // 请选择设备型号
    choiceEquipmentType: 'Please select type', // 请选择类型
    choiceEquipmentBrand: 'Please select brand', // 请选择品牌

    // 创建输入框提示
    inputEquipmentCode: 'Please enter the device model', // 请输入设备型号
    inputEquipmentRemark: 'Please enter device description', // 请输入设备描述
    // 创建编辑
    update: 'Edit device model',
    create: 'Create device model',

    // 品牌
    whole: 'Whole',
    NamgyaiVoluntarily: 'NamgyaiVoluntarily', // 朗杰
    Kardex: 'Kardex', // 卡迪斯
    Hanel: 'Hanel', // 享乃尔

    // 类型
    GoUpDecline: 'Elevator', // 升降库
    Rotation: 'Revolving storehouse' // 回转库
  },

  // 供应商
  Supply: {
    // 页面查找输入框提示
    SupplierSearch: 'Supplier code、Supplier name', // 供应商编码、供应商名称

    // 表头
    Code: 'SupplierCode', // 供应商编码
    Name: 'SupplierName', // 供应商名称
    Linkman: 'Contacts', // 联系人
    Phone: 'Phone', // 联系方式
    Address: 'Address', // 地址
    Remark: 'Remarks' // 备注
  },

  documentation: {
    documentation: 'Documentation',
    github: 'Github Repository'
  },
  permission: {
    addRole: 'New Role',
    editPermission: 'Edit',
    roles: 'Your roles',
    switchRoles: 'Switch roles',
    tips: 'In some cases, using v-permission will have no effect. For example: Element-UI  el-tab or el-table-column and other scenes that dynamically render dom. You can only do this with v-if.',
    delete: 'Delete',
    confirm: 'Confirm',
    cancel: 'Cancel'
  },
  guide: {
    description: 'The guide page is useful for some people who entered the project for the first time. You can briefly introduce the features of the project. Demo is based on ',
    button: 'Show Guide'
  },
  components: {
    documentation: 'Documentation',
    tinymceTips: 'Rich text is a core feature of the management backend, but at the same time it is a place with lots of pits. In the process of selecting rich texts, I also took a lot of detours. The common rich texts on the market have been basically used, and I finally chose Tinymce. See the more detailed rich text comparison and introduction.',
    dropzoneTips: 'Because my business has special needs, and has to upload images to qiniu, so instead of a third party, I chose encapsulate it by myself. It is very simple, you can see the detail code in @/components/Dropzone.',
    stickyTips: 'when the page is scrolled to the preset position will be sticky on the top.',
    backToTopTips1: 'When the page is scrolled to the specified position, the Back to Top button appears in the lower right corner',
    backToTopTips2: 'You can customize the style of the button, show / hide, height of appearance, height of the return. If you need a text prompt, you can use element-ui el-tooltip elements externally',
    imageUploadTips: 'Since I was using only the vue@1 version, and it is not compatible with mockjs at the moment, I modified it myself, and if you are going to use it, it is better to use official version.'
  },
  table: {
    dynamicTips1: 'Fixed header, sorted by header order',
    dynamicTips2: 'Not fixed header, sorted by click order',
    dragTips1: 'The default order',
    dragTips2: 'The after dragging order',
    title: 'Title',
    importance: 'Imp',
    type: 'Type',
    remark: 'Remark',
    search: 'Search',
    add: 'Add',
    export: 'Export',
    reviewer: 'reviewer',
    id: 'ID',
    date: 'Date',
    author: 'Author',
    readings: 'Readings',
    status: 'Status',
    actions: 'Actions',
    edit: 'Edit',
    publish: 'Publish',
    draft: 'Draft',
    delete: 'Delete',
    cancel: 'Cancel',
    confirm: 'Confirm'
  },
  example: {
    warning: 'Creating and editing pages cannot be cached by keep-alive because keep-alive include does not currently support caching based on routes, so it is currently cached based on component name. If you want to achieve a similar caching effect, you can use a browser caching scheme such as localStorage. Or do not use keep-alive include to cache all pages directly. See details'
  },
  errorLog: {
    tips: 'Please click the bug icon in the upper right corner',
    description: 'Now the management system are basically the form of the spa, it enhances the user experience, but it also increases the possibility of page problems, a small negligence may lead to the entire page deadlock. Fortunately Vue provides a way to catch handling exceptions, where you can handle errors or report exceptions.',
    documentation: 'Document introduction'
  },
  excel: {
    export: 'Export',
    selectedExport: 'Export Selected Items',
    placeholder: 'Please enter the file name (default excel-list)'
  },
  zip: {
    export: 'Export',
    placeholder: 'Please enter the file name (default file)'
  },
  pdf: {
    tips: 'Here we use window.print() to implement the feature of downloading PDF.'
  },
  theme: {
    change: 'Change Theme',
    documentation: 'Theme documentation',
    tips: 'Tips: It is different from the theme-pick on the navbar is two different skinning methods, each with different application scenarios. Refer to the documentation for details.'
  },
  tagsView: {
    refresh: 'Refresh',
    close: 'Close',
    closeOthers: 'Close Others',
    closeAll: 'Close All'
  },
  settings: {
    title: 'Page style setting',
    theme: 'Theme Color',
    tagsView: 'Open Tags-View',
    fixedHeader: 'Fixed Header',
    sidebarLogo: 'Sidebar Logo'
  }
}
