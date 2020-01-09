import React from 'react';
import { getSkierById } from '../../common/api';
import styled from 'styled-components';
import { SkierDetailPanel } from './SkierDetailPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { useStateAsync } from '../../hooks/useStateAsync';

const SkierGrid = styled.div`
    display: flex;
    height: fit-content;
    flex-direction: row;
    column-gap: ${props => props.theme.gap};
`;

export const SkierDetailView: React.FC<{ skierId: number }> = ({ skierId }) => {
    const [skier] = useStateAsync(getSkierById, skierId);

    return (
        <DetailViewWrapper
            url="/skier"
            backText="Zurück zur Fahrerübersicht"
            editConfig={{
                editText: 'Rennfahrer bearbeiten',
                editUrl: `/skier/${skierId}/update`
            }}
        >
            <SkierGrid>{skier && <SkierDetailPanel skier={skier} />}</SkierGrid>
        </DetailViewWrapper>
    );
};
