import React, { useState, useCallback } from 'react';
import styled from 'styled-components';
import { DefaultButton } from '../../theme/CustomComponents';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { useCallState } from '../../hooks/useCallState';
import { DeleteModal } from './DeleteModal';

const NegativeButton = styled(DefaultButton)`
    color: white;
    height: 100%;
    background-color: ${props => props.theme.negative};
    margin-left: ${props => props.theme.gap};
`;

const DeleteIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

export const DeleteModalHost: React.FC<{
    deleteText: string;
    deleteErrorMessage?: string;
    deleteFunc: () => Promise<void>;
}> = ({
    deleteText,
    deleteFunc,
    deleteErrorMessage = 'Fehler beim Löschen'
}) => {
    const [deleteChallenge, setDeleteChallenge] = useState(false);
    const { loading, error, setLoading, setError } = useCallState();

    const initDelete = useCallback(() => setDeleteChallenge(true), []);
    const cancelDelete = useCallback(() => {
        setDeleteChallenge(false);
        setLoading(false);
        setError(undefined);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const onDelete = useCallback(() => {
        const asyncWrapper = async () => {
            try {
                setError(undefined);
                setLoading(true);
                await deleteFunc();
            } catch (error) {
                setError(error);
            } finally {
                setLoading(false);
            }
        };

        asyncWrapper();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <>
            <NegativeButton onClick={initDelete}>
                <DeleteIcon icon={faTrash} />
                {deleteText}
            </NegativeButton>

            {deleteChallenge && (
                <DeleteModal
                    loading={loading}
                    error={error}
                    errorMessage={deleteErrorMessage}
                    onClose={cancelDelete}
                    onDelete={onDelete}
                />
            )}
        </>
    );
};
