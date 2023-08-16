
/**
 *  LangModel.js v1.0.0
 *  From Rugal Tu
 * */

class LangModel {

    constructor() {
        this.LangKey = {};
        this.DefaultLang = 'shared';
        this.CurrentLang = this.DefaultLang;
    }

    Get(Key, Lang = this.CurrentLang) {
        Lang = Lang.toLowerCase();
        let LangDic = this.LangKey[Lang];
        if (LangDic == undefined)
            return Key;

        let Value = LangDic[Key];
        Value = Value ?? Key;
        return Value;
    }

    Add(KeyValue, Lang = this.CurrentLang) {
        Lang = Lang.toLowerCase();
        let LangDic = this.LangKey[Lang];
        if (LangDic == undefined) {
            this.LangKey[Lang] = {};
            LangDic = this.LangKey[Lang];
        }
        this.LangKey[Lang] = {
            ...LangDic,
            ...KeyValue
        };

        return this;
    }

    GetAll(Lang = this.CurrentLang) {
        Lang = Lang.toLowerCase();
        let LangDic = this.LangKey[Lang];
        return LangDic;
    }

    AsLang(Lang = this.CurrentLang) {
        let NewModel = new LangModel();
        NewModel.LangKey = this.LangKey;
        NewModel.SetLang(Lang);
        return NewModel;
    }

    SetLang(Lang) {
        this.CurrentLang = Lang.toLowerCase();
        return this;
    }
}

const Lang = new LangModel();