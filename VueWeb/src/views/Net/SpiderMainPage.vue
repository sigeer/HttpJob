<template>
    <a-card>
        <div>
            <a-form :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-divider>基础</a-divider>
                <a-form-item label="资源站点">
                    <a-select v-model="selectedId" style="width: 100%">
                        <a-select-option v-for="x in dataSource" :key="x.id">
                            {{ x.name }}
                        </a-select-option>
                    </a-select>
                </a-form-item>
                <template v-if="selectedItem">
                    <a-form-item label="Url">
                        {{ selectedItem.Url }}
                        <a @click="showEditor"> 编辑</a>
                        <a-divider type="vertical"></a-divider>
                        <a>删除</a>
                    </a-form-item>
                </template>
                <a-divider>爬虫</a-divider>
                <a-form-item label="选择爬虫">
                    <a-select v-model="spiderId" style="width: 100%">
                        <a-select-option v-for="x in spiderList" :key="x.id">
                            {{ x.name }}
                        </a-select-option>
                    </a-select>
                </a-form-item>
            </a-form>
        </div>
        <template v-if="showResult">
            <a-divider></a-divider>
        </template>
    </a-card>
</template>

<script>
    import { pageMixins } from '../../utils/page'
    export default {
        mixins: [pageMixins],

        data() {
            return {
                dataSource: [],
                selectedId: null,
                spiderId: null,

                spiderList: [],
                ruleList: [],
                showResult: false
            };
        },

        computed: {
            selectedItem() {
                return this.dataSource.find((x) => x.id === this.selectedId);
            },
        },

        methods: {
            getDataSource() { },

            getRuleList() { },

            showEditor(m) { },

            showRuleEditor(m) { },
        },
    };
</script>