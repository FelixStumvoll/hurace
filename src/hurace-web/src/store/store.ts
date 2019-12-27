import { createBrowserHistory } from 'history';
import { routerMiddleware } from 'connected-react-router';
import { applyMiddleware, createStore, compose } from 'redux';
import { rootReducer } from './rootReducer';

export const history = createBrowserHistory();

export const store = createStore(
    rootReducer(history),
    compose(applyMiddleware(routerMiddleware(history)))
);
