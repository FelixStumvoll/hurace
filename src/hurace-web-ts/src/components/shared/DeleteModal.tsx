import React from 'react';
import { Modal } from './Modal';
import styled from 'styled-components';
import {
    DefaultButton,
    RowFlex,
    ColumnFlex,
    FormErrorMessage
} from '../../theme/CustomComponents';
import { LoadingWrapper } from './LoadingWrapper';
import { error } from 'console';

const DeleteQuestion = styled.span`
    font-weight: bold;
    width: fit-content;
    margin: 0 auto ${props => props.theme.gap} auto;
`;

const ModalButton = styled(DefaultButton)`
    color: white;
    height: 100%;
    width: 100px;
`;

const PositiveButton = styled(ModalButton)`
    background-color: ${props => props.theme.positive};
`;

const NegativeButton = styled(ModalButton)`
    background-color: ${props => props.theme.negative};
    margin-left: ${props => props.theme.gap};
`;

const DeleteFlex = styled(RowFlex)`
    height: 35px;
    justify-content: center;
`;

const MarginWrapper = styled.div`
    margin-bottom: ${props => props.theme.gap};
`;

const ModalColumn = styled(ColumnFlex)``;

export const DeleteModal: React.FC<{
    loading: boolean;
    error: Error | undefined;
    errorMessage: string;
    onDelete: () => void;
    onClose: () => void;
}> = ({ error, loading, errorMessage, onDelete, onClose }) => {
    return (
        <Modal>
            <ModalColumn>
                <DeleteQuestion>Löschen ?</DeleteQuestion>
                {loading && (
                    <MarginWrapper>
                        <LoadingWrapper
                            loading={loading}
                            error={error}
                            errorMessage={errorMessage}
                        />
                    </MarginWrapper>
                )}

                {error && (
                    <MarginWrapper>
                        <FormErrorMessage>{errorMessage}</FormErrorMessage>
                    </MarginWrapper>
                )}
                <DeleteFlex>
                    <PositiveButton onClick={onDelete}>Ja</PositiveButton>
                    <NegativeButton onClick={onClose}>Nein</NegativeButton>
                </DeleteFlex>
            </ModalColumn>
        </Modal>
    );
};
