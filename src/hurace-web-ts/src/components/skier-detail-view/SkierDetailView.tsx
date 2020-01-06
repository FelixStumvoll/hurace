import React from 'react';
import { getSkierById } from '../../common/api';
import styled from 'styled-components';
import { SkierDetailPanel } from './SkierDetailPanel';
import { Card } from '../../theme/StyledComponents';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { useStateAsync } from '../../hooks/asyncState';

const SkierGrid = styled.div`
    display: flex;
    flex-direction: column;
    column-gap: 10px;
`;

const SkierDetailCard = styled(Card)`
    display: flex;
    padding: 0px;
    flex-direction: row;
    width: fit-content;
    max-width: 100%;
`;

export const SkierDetailView: React.FC<{ skierId: number }> = ({ skierId }) => {
    const [skier] = useStateAsync(getSkierById, skierId);

    return (
        <DetailViewWrapper url="/skier" backText="Zurück zur Fahrerübersicht">
            <SkierGrid>
                {skier && (
                    <SkierDetailCard>
                        {skier.imageUrl && (
                            <img alt="Skier" src={skier.imageUrl} />
                        )}
                        <SkierDetailPanel skier={skier} />
                    </SkierDetailCard>
                )}
            </SkierGrid>
        </DetailViewWrapper>
    );
};
