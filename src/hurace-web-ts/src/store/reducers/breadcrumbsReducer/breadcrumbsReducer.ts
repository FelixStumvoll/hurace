import {Breadcrumb} from "../../../interfaces/Breadcrumb";
import {BreadcrumbsAction, BreadcrumbsActionType} from "./breadcrumbsActionType";
import {Reducer} from "react";

export interface BreadcrumbsReducerState {
    breadcrumbList: Breadcrumb[]
}

const initialState: BreadcrumbsReducerState = {
    breadcrumbList: []
};

export const breadcrumbsReducer: Reducer<BreadcrumbsReducerState, BreadcrumbsAction> =
    (state = initialState, action: BreadcrumbsAction): BreadcrumbsReducerState => {
        switch (action.type) {
            case BreadcrumbsActionType.Add:
                return {
                    breadcrumbList: [...state.breadcrumbList, action.breadcrumb]
                };
            case BreadcrumbsActionType.Remove:
                let removeIndex = state.breadcrumbList.indexOf(action.breadcrumb);
                return {
                    breadcrumbList: [...state.breadcrumbList.slice(0, removeIndex + 1)]
                };
            case BreadcrumbsActionType.Clear:
                return {
                    breadcrumbList: []
                };
            default: return state
        }
    };