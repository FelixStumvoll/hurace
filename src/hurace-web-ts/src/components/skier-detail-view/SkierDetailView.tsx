import React, { useCallback } from 'react';
import { getSkierById } from '../../common/api';
import styled from 'styled-components';
import { SkierDetailPanel } from './SkierDetailPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { useStateAsync } from '../../hooks/useStateAsync';
import { RowFlex } from '../../theme/CustomComponents';

const SkierGrid = styled(RowFlex)`
    height: fit-content;
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
