import React from 'react';

type WrappedInputProps<T> = {
    onInputChange : () => T
} & React.DetailedHTMLProps<
    React.InputHTMLAttributes<HTMLInputElement>,
    HTMLInputElement
>;

export const WrappedInput: <T>(p: WrappedInputProps<T>) => React.ReactElement<WrappedInputProps<T>> = ({onInputChange, ...props}) => {
    return <input></input>;
};
