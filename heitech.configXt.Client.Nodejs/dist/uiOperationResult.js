"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.UiOperationResult = void 0;
class UiOperationResult {
    constructor(isSuccess, resultName, errorMessage, configurationModels) {
        this.isSuccess = isSuccess;
        this.resultName = resultName;
        this.errorMessage = errorMessage;
        this.configurationModels = configurationModels;
    }
}
exports.UiOperationResult = UiOperationResult;
