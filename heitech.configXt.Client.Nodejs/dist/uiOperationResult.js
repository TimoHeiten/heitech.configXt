"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.UiOperationResult = void 0;
class UiOperationResult {
    constructor(IsSuccess, ResultName, ErrorMessage, ConfigurationModels) {
        this.IsSuccess = IsSuccess;
        this.ResultName = ResultName;
        this.ErrorMessage = ErrorMessage;
        this.ConfigurationModels = ConfigurationModels;
    }
    format() {
        let models = this.ConfigurationModels.map(c => `${c.Name} --> ${c.Value}`).join("|");
        return `Success:[${this.IsSuccess}]\nResult:[${this.ResultName}]\nerror:[${this.ErrorMessage}]\nModels:[${models}]`;
    }
    static FromMsg(result) {
        return new UiOperationResult(result.IsSuccess, result.ResultName, result.ErrorMessage, result.ConfigurationModels);
    }
}
exports.UiOperationResult = UiOperationResult;
