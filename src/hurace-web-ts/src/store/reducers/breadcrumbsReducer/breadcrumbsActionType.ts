import {Breadcrumb} from "../../../interfaces/Breadcrumb";

export enum BreadcrumbsActionType {
    Add,
    Remove,
    Clear
}

export type BreadcrumbsAction =
    | { type: BreadcrumbsActionType.Add | BreadcrumbsActionType.Remove, breadcrumb: Breadcrumb }
    | { type: BreadcrumbsActionType.Clear };