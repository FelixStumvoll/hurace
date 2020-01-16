import React, { useState, useCallback } from 'react';
import styled from 'styled-components';
import {
    DefaultButton,
    AlignRight,
    RowFlex,
    ColumnFlex
} from '../../theme/CustomComponents';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { Modal } from './Modal';

const PositiveButton = styled(DefaultButton)`
    color: white;
    height: 100%;
    background-color: ${props => props.theme.positive};
`;

const NegativeButton = styled(DefaultButton)`
    color: white;
    height: 100%;
    background-color: ${props => props.theme.negative};
    margin-left: ${props => props.theme.gap};
`;

const DeleteIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const DeleteQuestion = styled.span`
    font-weight: bold;
    width: fit-content;
    margin: 0 auto ${props => props.theme.gap} auto;
`;

const DeleteFlex = styled(RowFlex)`
    height: 35px;
    justify-content: center;
`;

const ModalColumn = styled(ColumnFlex)``;

export const DeleteBar: React.FC<{
    deleteText: string;
    deleteFunc: () => void;
}> = ({ deleteText, deleteFunc }) => {
    const [deleteChallenge, setDeleteChallenge] = useState(false);

    const initDelete = useCallback(() => setDeleteChallenge(true), []);
    const cancelDelete = useCallback(() => setDeleteChallenge(false), []);

    return (
        <AlignRight>
            {deleteChallenge ? (
                <Modal>
                    <ModalColumn>
                        <DeleteQuestion>Löschen ?</DeleteQuestion>

                        <DeleteFlex>
                            <PositiveButton onClick={deleteFunc}>
                                Ja
                            </PositiveButton>
                            <NegativeButton onClick={cancelDelete}>
                                Nein
                            </NegativeButton>
                        </DeleteFlex>
                    </ModalColumn>
                </Modal>
            ) : (
                <NegativeButton onClick={initDelete}>
                    <DeleteIcon icon={faTrash} />
                    {deleteText}
                </NegativeButton>
            )}
        </AlignRight>
    );
};
