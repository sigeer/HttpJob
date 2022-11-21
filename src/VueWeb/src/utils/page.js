export const commonMixins = {
  data() {
    return {
      labelCol: {
        xs: { span: 24 },
        sm: { span: 5 },
      },
      wrapperCol: {
        xs: { span: 24 },
        sm: { span: 16 },
      },
    }
  },
}
export const pageMixins= {
  mixins: [commonMixins],
  data() {
    return {
      paginationModel: {
        current: 1,
        pageSize: 10,
        total: 0,
        showSizeChanger: true,
        showTotal: (total, range) => `显示第 ${range[0]} 条到 ${range[1]} 条数据，共 ${total} 条数据`
      },
      state: {
        isTableLoading: false,
      },
    }
  },
  created() {
    this.pageLoad && this.pageLoad()
  },
  methods: {
    getDataSource() {},
    search() {
      this.paginationModel.current = 1
      this.getDataSource()
    },
    handleTableChange (pagination, filters, sorter) {
      this.paginationModel.current = pagination.current
      this.paginationModel.pageSize = pagination.pageSize
      this.getDataSource()
    }
  },
}

export const editorMixins = {
  mixins: [commonMixins],
  data() {
    return {
      visible: false,
      state: {
        isSubmitting: false,
      },
      form: this.$form.createForm(this),
    }
  },
  methods: {
    _show() {
      this.visible = true
    },
    _hide() {
      this.visible = false
    }
  }
}
