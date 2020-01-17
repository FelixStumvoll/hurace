import React from 'react';
import styled from 'styled-components';
import { DefaultButton, FormErrorMessage } from '../../theme/CustomComponents';

const CrudButton = styled(DefaultButton)`
    flex-grow: 1;
    color: white;
`;

const CancelButton = styled(CrudButton)`
    margin-right: 5px;
    background-color: ${props => props.theme.negative};
`;

const SaveButton = styled(CrudButton)`
    margin-left: 5px;
    background-color: ${props => props.theme.positive};

    :disabled {
        background-color: ${props => props.theme.gray};
        cursor: not-allowed;
    }
`;

const ButtonPanel = styled.div`
    display: flex;
    justify-content: center;
    margin-top: 10px;
`;

const Form = styled.form`
    display: flex;
    flex-direction: column;
`;

const FormError = styled(FormErrorMessage)`
    margin-top: 10px;
`;

export const FormWrapper: React.FC<{
    onCancel: () => void;
    isSubmitting: boolean;
    onSubmit: (e?: React.FormEvent<HTMLFormElement> | undefined) => void;
    error?: string;
}> = ({ children, onCancel, isSubmitting, onSubmit, error }) => {
    return (
        <Form onSubmit={onSubmit}>
            {children}

            {error && <FormError>{error}</FormError>}
            <ButtonPanel>
                <CancelButton onClick={onCancel}>Abbrechen</CancelButton>
                <SaveButton disabled={isSubmitting} type="submit">
                    Speichern
                </SaveButton>
            </ButtonPanel>
        </Form>
    );
};
