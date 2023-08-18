
/**
 *  LangModel.js v1.0.2.3
 *  From Rugal Tu
 * */

class LangModel {

    constructor() {
        this.LangKey = {};
        this.CurrentLang = 'shared';
    }

    Get(Key, Lang = this.CurrentLang) {
        Lang = Lang.toLowerCase();
        let LangDic = this.LangKey[Lang];
        if (LangDic == null)
            return Key;

        let Value = LangDic[Key];
        Value = Value ?? Key;
        return Value;
    }

    GetList(Keys = [], Lang = this.CurrentLang) {
        Lang = Lang.toLowerCase();

        let LangDic = this.LangKey[Lang];
        if (LangDic == null)
            return Keys;

        let Values = Keys.map(Item => LangDic[Item] ?? Item);
        return Values
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

    WithClone(Lang = this.CurrentLang) {
        let NewModel = new LangModel();
        NewModel.LangKey = this.LangKey;
        NewModel.WithLang(Lang);
        return NewModel;
    }

    WithLang(Lang) {
        this.CurrentLang = Lang.toLowerCase();
        return this;
    }
}

const Lang = new LangModel();